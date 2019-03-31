using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface ICallExpression
    {
        MethodCallExpression _callExpression { get;  }
    }
}