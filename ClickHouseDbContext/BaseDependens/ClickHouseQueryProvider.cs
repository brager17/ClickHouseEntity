using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DbContext
{
    public class ClickHouseQueryProvider : IQueryProvider
    {
        private readonly IExpressionsToObject _expressionsToObject;

        public ClickHouseQueryProvider(IExpressionsToObject expressionsToObject)
        {
            _expressionsToObject = expressionsToObject;
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

        public TResult Execute<TResult>(Expression expression)
        {
            if (!typeof(TResult).IsGenericType ||
                !(typeof(TResult).GetGenericTypeDefinition() == typeof(IEnumerator<>)))
                throw new ArgumentException();

            var execute = _expressionsToObject.Handle<TResult>(expression);
            return execute;
        }
    }
}