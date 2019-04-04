using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class VisitorsHandler : IVisitorHandler
    {
        private readonly PropertyMapInfoVisitor _propertyMapInfoVisitor;
        private readonly AggregateLinqVisitor _aggregateLinqVisitor;

        public VisitorsHandler(PropertyMapInfoVisitor propertyMapInfoVisitor, AggregateLinqVisitor aggregateLinqVisitor)
        {
            _propertyMapInfoVisitor = propertyMapInfoVisitor;
            _aggregateLinqVisitor = aggregateLinqVisitor;
        }

        public AggregateLinqInfo Handle(Expression expression)
        {
            var selectInfos = _propertyMapInfoVisitor.GetInfo(expression).SelectInfos;
            return _aggregateLinqVisitor.GetInfo(selectInfos, expression);
        }
    }
}    