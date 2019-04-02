using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickDbContextInfrastructure;
using DbContext;
using ReflectionCache;
using Root;

namespace Context
{
    public class ClickHouseDbContext : IClickHouseDbContext
    {
        private string ConnectionString { get; set; }

        protected ClickHouseDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            GetType().GetProperties()
                .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToList()
                .ForEach(x =>
                {
                    var propertyType = x.PropertyType.GenericTypeArguments.First();
                    x.SetValue(this, Activator.CreateInstance(typeof(DbSet<>).MakeGenericType(propertyType),
                        new ClickHouseQueryProvider(GetClickHouseProvider.Get(connectionString))));
                });
        }

        public class DbSet<T> : ClickHouseQueryable<T>, IDbSet
        {
            public DbSet(IQueryProvider queryProvider) :
                base(GetInitialExpressionByType<T>(), queryProvider)
            {
            }
        }

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
                .Invoke(null, new Type[] {typeof(T), typeof(T)});
            var callExpression =
                Expression.Call((Expression) null, SelectMethod, new T[] { }.AsQueryable().Expression,
                    Expression.Quote(lambda));
            return callExpression;
        }
    }
}