using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IExpressionToLinqInfoHandler<TInfo>
    {
        TInfo GetLinqInfo<T>(Expression<T> expression);
    }
}