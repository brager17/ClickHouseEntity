using System;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace DbContext
{
    public class LambdaCompileQuery<T> : IQuery<LambdaExpression, T>
    {
        public T Query(LambdaExpression input) => (T) (object) input.Compile();
    }
}