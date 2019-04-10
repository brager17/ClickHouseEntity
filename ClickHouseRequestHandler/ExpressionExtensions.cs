using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ExpressionTreeVisitor
{
    public static class ExpressionExtensions
    {
        public static ParameterExpression Parameter(this LambdaExpression expression) => expression.Parameters.Single();
    }
}