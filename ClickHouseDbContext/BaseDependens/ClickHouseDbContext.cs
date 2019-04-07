using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ClickDbContextInfrastructure;
using DbContext;
using ReflectionCache;
using Root;

namespace Context
{
    public class ClickHouseDbContext : IClickHouseDbContext
    {
        private string ConnectionString { get; }

        protected ClickHouseDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            // initial dbSets
            GetType().GetProperties()
                .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToList()
                .ForEach(x =>
                {
                    var propertyType = x.PropertyType.GenericTypeArguments.First();
                    var getDbSetOperations =
                        typeof(GetDbSet).GetMethods().Single(xx => xx.Name == "GetDbSetOperations")
                            .MakeGenericMethod(propertyType);
                    x.SetValue(this, Activator.CreateInstance(typeof(DbSet<>).MakeGenericType(propertyType),
                        new ClickHouseQueryProvider(
                            GetClickHouseProvider.Get(connectionString, _dbLoggers)),
                        getDbSetOperations.Invoke(null, new object[] {connectionString, _dbLoggers})));
                });
        }

        public class DbSet<T> : ClickHouseQueryable<T>, IDbSet, IDbSet<T>
        {
            private readonly IDbSetOperations<T> _operations;

            public DbSet(IQueryProvider queryProvider, IDbSetOperations<T> operations) :
                base(GetInitialExpressionByType<T>(), queryProvider)
            {
                _operations = operations;
            }

            public void Add(IEnumerable<T> items) => _operations.Add(items);

            public void Remove(IEnumerable<T> item)
            {
                if (_enumerator == null) throw new ArgumentException();
                _operations.Remove(item, _enumerator);
            }

            public void Remove(Expression<Func<T, bool>> exprFilter) => _operations.Remove(exprFilter);

            public void SaveChanges() => _operations.SaveChanges();
        }

        #region helpers

        protected virtual IEnumerable<IDbLogger> _dbLoggers => new List<IDbLogger> {new StubDBLogger()};

        private static MethodCallExpression GetInitialExpressionByType<T>()
        {
            var parameter = Expression.Parameter(typeof(T));
            var props = typeof(T).GetProperties().ToList();
            var members = props.Select(x => Expression.Property(parameter, x));
            var assignments = props.Zip(members, Expression.Bind);
            var newExp = Expression.New(typeof(T));
            var memberInit = Expression.MemberInit(newExp, assignments);
            var lambda = Expression.Lambda<Func<T, T>>(memberInit, parameter);
            var SelectMethod = (MethodInfo) new CachedReflectionInfoTypeWrapper().GetMethod("Select_TSource_TResult_2")
                .Invoke(null, new[] {typeof(T), typeof(T)});
            var callExpression = Expression.Call(null, SelectMethod, new T[] { }.AsQueryable().Expression,
                Expression.Quote(lambda));
            return callExpression;
        }

        #endregion helpers
    }
}