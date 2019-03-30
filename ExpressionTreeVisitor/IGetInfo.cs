using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IGetInfo<T>
    {
        T GetInfo(Expression expression);
    }
}