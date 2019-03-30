using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using DbContext;
using ExpressionTreeVisitor;

namespace Context
{
    public class ClickHouseDbContext
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
                        new ClickHouseQueryProvider(
                            new SqlToObject(
                                new ExpressionToSqlConverter(new SqlRequestHandler(
                                        new SelectRequestHandler(new GetBindingInfo()),
                                        new TableNameRequestHandler()),
                                    new BaseExpressionVisitor(new GetBindingInfo())),
                                new DbHandler(ConnectionString,
                                    new ModelBinder(new ValueTypeHandler(),
                                        new ClassTypeHandler(new EntityRowToObject()))),
                                new BaseExpressionVisitor(new GetBindingInfo())))));
                });
        }

        public class DbSet<T> : ClickHouseQueryable<T>
        {
            public DbSet(IQueryProvider queryProvider) :
                base(Expression.Constant(new T[] { }.AsQueryable()), queryProvider)
            {
            }
        }
    }
}