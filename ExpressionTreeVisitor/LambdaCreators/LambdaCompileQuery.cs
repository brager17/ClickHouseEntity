using System;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace DbContext
{
    public class LambdaCompileQuery : IQuery<LambdaExpression, Delegate>
    {
        public Delegate Query(LambdaExpression input) => input.Compile();
    }
}