using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ClickDbContextInfrastructure;
using DbContext;
using ExpressionTreeVisitor;
using Root;

namespace Context
{
    public class ClickHouseDbContext:IClickHouseDbContext
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

        public class DbSet<T> : ClickHouseQueryable<T>,IDbSet
        {
            public DbSet(IQueryProvider queryProvider) :
                base(Expression.Constant(new T[] { }.AsQueryable()), queryProvider)
            {
            }
        }
    }
}