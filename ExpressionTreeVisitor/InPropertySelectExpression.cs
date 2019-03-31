using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class InPropertySelectExpression : ICallExpression, IMemberExpression, IConstantExpression
    {
        public InPropertySelectExpression(MethodCallExpression callExpression)
        {
            _callExpression = callExpression;
        }

        public InPropertySelectExpression(MemberExpression memberExpression)
        {
            _memberExpression = memberExpression;
        }

        public InPropertySelectExpression(ConstantExpression contantExpression)
        {
            _contantExpression = contantExpression;
        }
        public MethodCallExpression _callExpression { get;  }
        public MemberExpression _memberExpression { get;  }
        public ConstantExpression _contantExpression { get;  }
    }

 
}