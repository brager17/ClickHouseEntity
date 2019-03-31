using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DbContext
{
    public class ClickHouseQueryProvider : IQueryProvider
    {
        private readonly ISqlToObject _sqlToObject;

        public ClickHouseQueryProvider(ISqlToObject sqlToObject)
        {
            _sqlToObject = sqlToObject;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new ClickHouseQueryable<TElement>(expression, this);

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression) => _sqlToObject.Handle<TResult>(expression);
        

    }
}