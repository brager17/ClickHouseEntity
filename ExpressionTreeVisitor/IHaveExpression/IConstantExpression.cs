using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IConstantExpression
    {
        ConstantExpression _contantExpression { get; set; }
    }
}