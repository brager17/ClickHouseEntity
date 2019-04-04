using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class WhereQuery : IQuery<LambdaListSelectInfo, AggregateLinqInfo>
    {
        private WhereToSqlVisitor _whereToSqlVisitor { get; set; }

        public WhereQuery(WhereToSqlVisitor whereToSqlVisitor)
        {
            _whereToSqlVisitor = whereToSqlVisitor;
        }

        public AggregateLinqInfo Query(LambdaListSelectInfo input)
        {
            if (input.AggregateLinqInfo.OrderInfo == null) input.AggregateLinqInfo.WhereInfo = new List<WhereInfo>();

            var lambda = (LambdaExpression) ((UnaryExpression) input.MethodCallExpression.Arguments.Last()).Operand;

            input.AggregateLinqInfo.WhereInfo.Add(new WhereInfo
                {WhereStr = _whereToSqlVisitor.Visit(input.SelectInfos, lambda)});

            return input.AggregateLinqInfo;
        }
    }
}