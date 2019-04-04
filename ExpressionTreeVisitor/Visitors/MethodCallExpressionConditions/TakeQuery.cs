using System;
using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class TakeQuery : IQuery<LambdaListSelectInfo, AggregateLinqInfo>
    {
        public AggregateLinqInfo Query(LambdaListSelectInfo input)
        {
            if (input?.AggregateLinqInfo == null)
                throw new ArgumentException();
            var constant = (ConstantExpression) input.MethodCallExpression.Arguments.Last();
            var takeInfo = new TakeInfo(Convert.ToInt64(constant.Value));
            input.AggregateLinqInfo.TakeInfo = takeInfo;
            return input.AggregateLinqInfo;
        }
    }

    public class DefaultQuery : IQuery<LambdaListSelectInfo, AggregateLinqInfo>
    {
        public AggregateLinqInfo Query(LambdaListSelectInfo input)
        {
            return input.AggregateLinqInfo;
        }
    }
}