using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class InPropertySelectExpression : ICallExpression, IMemberExpression, IConstantExpression
    {
        public MethodCallExpression _callExpression { get; set; }
        public MemberExpression _memberExpression { get; set; }
        public ConstantExpression _contantExpression { get; set; }
    }

 
}