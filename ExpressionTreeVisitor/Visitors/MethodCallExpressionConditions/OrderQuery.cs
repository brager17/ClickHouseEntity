using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class OrderQuery : IQuery<LambdaListSelectInfo, AggregateLinqInfo>
    {
        private readonly OrderingToSqlVisitor _orderingToSqlVisitor;

        public OrderQuery(OrderingToSqlVisitor _orderingToSqlVisitor)
        {
            this._orderingToSqlVisitor = _orderingToSqlVisitor;
        }

        public AggregateLinqInfo Query(LambdaListSelectInfo input)
        {
            if (input?.AggregateLinqInfo == null)
                throw new ArgumentException();
            if (input.AggregateLinqInfo.OrderInfo == null) input.AggregateLinqInfo.OrderInfo = new List<OrderInfo>();

            var lambda = (LambdaExpression) ((UnaryExpression) input.MethodCallExpression.Arguments.Last()).Operand;

            var orderInfo = new OrderInfo
            {
                OrderType = Enum.Parse<OrderType>(input.MethodCallExpression.Method.Name),
                OrderString = _orderingToSqlVisitor.Visit(input.SelectInfos, lambda)
            };

            input.AggregateLinqInfo.OrderInfo.Add(orderInfo);
            return input.AggregateLinqInfo;
        }
    }
}