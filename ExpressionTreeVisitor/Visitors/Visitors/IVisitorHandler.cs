using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IVisitorHandler
    {
        AggregateLinqInfo Handle(Expression expression);
    }
}