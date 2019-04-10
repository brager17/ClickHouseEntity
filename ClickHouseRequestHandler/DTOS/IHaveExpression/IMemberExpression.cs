using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public interface IMemberExpression
    {
        MemberExpression _memberExpression { get;  }
    }
}