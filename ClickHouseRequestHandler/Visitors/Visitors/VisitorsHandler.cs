using System;
using System.Diagnostics;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class VisitorsHandler : IQuery<Expression, AggregateLinqInfo>
    {
        private readonly PropertyMapInfoVisitor _propertyMapInfoVisitor;
        private readonly IQuery<AggregateLinqVisitorDto, AggregateLinqInfo> _aggregateLinqVisitor;

        public VisitorsHandler(
            PropertyMapInfoVisitor propertyMapInfoVisitor,
            IQuery<AggregateLinqVisitorDto, AggregateLinqInfo> aggregateLinqVisitor)
        {
            _propertyMapInfoVisitor = propertyMapInfoVisitor;
            _aggregateLinqVisitor = aggregateLinqVisitor;
        }

        public AggregateLinqInfo Query(Expression expression)
        {
            var selectInfos = _propertyMapInfoVisitor.Query(expression).SelectInfos;
            var aggregateLinqInfo = _aggregateLinqVisitor.Query(new AggregateLinqVisitorDto
                {selectInfos = selectInfos, expression = expression});
            return aggregateLinqInfo;
        }
    }
}