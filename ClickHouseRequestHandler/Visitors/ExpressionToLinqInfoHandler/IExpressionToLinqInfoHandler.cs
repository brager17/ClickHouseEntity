using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IExpressionToLinqInfoHandler<TInfo>
    {
        TInfo GetLinqInfo(LambdaExpression expression);
    }
}