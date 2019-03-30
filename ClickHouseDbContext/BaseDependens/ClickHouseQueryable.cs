using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ClickHouseQueryable<T> : IQueryable<T>
    {
        public ClickHouseQueryable(Expression expression, IQueryProvider queryProvider)
        {
            Expression = expression;
            Provider = queryProvider;
        }

        public IEnumerator<T> GetEnumerator() => Provider.Execute<IEnumerator<T>>(Expression);


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => typeof(T);
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }
    }
}