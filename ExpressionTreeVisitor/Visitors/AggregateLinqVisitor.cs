using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickDbContextInfrastructure;

namespace ExpressionTreeVisitor
{
    public class AggregateLinqVisitor : ExpressionVisitor
    {
        private readonly OrderingToSqlVisitor _orderingToSqlVisitor;
        private AggregateLinqInfo AggregateLinqInfo { get; set; }

        private WhereToSqlVisitor _whereToSqlVisitor { get; set; }
        private PropertyMapInfoVisitor PropertyMapInfoVisitor { get; set; }

        private IEnumerable<SelectInfo> _selectInfos { get; set; }

        public AggregateLinqVisitor(
            WhereToSqlVisitor whereToSqlVisitor,
            PropertyMapInfoVisitor propertyMapInfoVisitor,
            OrderingToSqlVisitor orderingToSqlVisitor)
        {
            _orderingToSqlVisitor = orderingToSqlVisitor;
            _whereToSqlVisitor = whereToSqlVisitor;
            PropertyMapInfoVisitor = propertyMapInfoVisitor;
        }

        public AggregateLinqInfo GetInfo(IEnumerable<SelectInfo> selectInfos, Expression expression)
        {
            AggregateLinqInfo = new AggregateLinqInfo();
            AggregateLinqInfo.SelectInfo = (_selectInfos = selectInfos).ToList();
            base.Visit(expression);
            return AggregateLinqInfo;
        }

        //todo add dictionary
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var methodName = node.Method.Name;
            var innerLambda = node.Arguments.Last();
            if (methodName == "Where" && innerLambda is UnaryExpression unary &&
                unary.Operand is LambdaExpression lambda)
                AggregateLinqInfo.WhereInfo.Add(new WhereInfo
                    {WhereStr = _whereToSqlVisitor.Visit(_selectInfos, lambda)});

            if (methodName == "Take" && innerLambda is ConstantExpression contant)
                AggregateLinqInfo.TakeInfo = new TakeInfo(Convert.ToInt64(contant.Value));

            if (new[] {"OrderBy", "OrderByDescending"}.Contains(methodName) &&
                innerLambda is UnaryExpression unaryExpression &&
                unaryExpression.Operand is LambdaExpression lambdaExpression)
                AggregateLinqInfo.OrderInfo.Add(new OrderInfo
                {
                    OrderType = Enum.Parse<OrderType>(methodName),
                    OrderString = _orderingToSqlVisitor.Visit(_selectInfos, lambdaExpression)
                });

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            // if node.type is DbSetType
            if (node.Type.IsGenericType && node.Type.GenericTypeArguments.Length == 1 &&
                node.Type.GenericTypeArguments.Single().IsDbSetType())
                AggregateLinqInfo.SetType = node.Type.GetGenericArguments().Single();
            return base.VisitConstant(node);
        }
    }
}