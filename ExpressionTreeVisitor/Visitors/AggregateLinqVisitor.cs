using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class AggregateLinqVisitor : ExpressionVisitor
    {
        private readonly ConditionQuery<LambdaListSelectInfo, AggregateLinqInfo> _aggregateLinqInfoQuery;
        private readonly OrderingToSqlVisitor _orderingToSqlVisitor;
        private AggregateLinqInfo AggregateLinqInfo { get; set; }


        private IEnumerable<SelectInfo> _selectInfos { get; set; }

        public AggregateLinqVisitor(ConditionQuery<LambdaListSelectInfo, AggregateLinqInfo> aggregateLinqInfoQuery)
        {
            _aggregateLinqInfoQuery = aggregateLinqInfoQuery;
        }

        public AggregateLinqInfo GetInfo(IEnumerable<SelectInfo> selectInfos, Expression expression)
        {
            AggregateLinqInfo = new AggregateLinqInfo();
            AggregateLinqInfo.SelectInfo = (_selectInfos = selectInfos).ToList();
            base.Visit(expression);
            return AggregateLinqInfo;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            _aggregateLinqInfoQuery.Query(new LambdaListSelectInfo{
                SelectInfos = _selectInfos, AggregateLinqInfo = AggregateLinqInfo, MethodCallExpression = node});
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