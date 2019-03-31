using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class UnaryExpressionToLinqInfoHandler : IExpressionToLinqInfoHandler<WhereInfo>
    {
        public WhereInfo GetLinqInfo<T>(Expression<T> expression)
        {
            throw new System.NotImplementedException();
        }
    }

    public class WhereVisitor : ExpressionVisitor, IGetInfo<WhereInfo>
    {

        public string WhereStr { get; set; } = string.Empty;
        private WhereInfo WhereInfo { get; set; }

        private MemberExpression MemberExpression { get; set; }

        private ConstantExpression _constantExpression { get; set; }
        
        private List<BetweenUnaryOperationOperator> orAndOperators { get; set; }

        public WhereInfo GetInfo(Expression expression)
        {
            orAndOperators = new List<BetweenUnaryOperationOperator>();
            base.Visit(expression);
            return WhereInfo;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            // если это полная лябмда их Where
               
            if (!(node.Left is BinaryExpression) && !(node.Right is BinaryExpression))
            {
                var left = GetExpressionInfo((dynamic) node.Left);
                var right = GetExpressionInfo((dynamic) node.Right);
                var @operator = node.NodeType.GetBetweenUnaryOperandOperator();
            }

            return base.VisitBinary(node);
        }

        private UnaryOperand GetExpressionInfo(ConstantExpression node)
        {
            return new UnaryOperand(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _constantExpression = node;
            return node;
        }

        private UnaryOperand GetExpressionInfo(MemberExpression node)
        {
            return new UnaryOperand(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            MemberExpression = node;
            return base.VisitMember(node);
        }

        private UnaryOperand GetExpressionInfo(MethodCallExpression node)
        {
            // todo вызвать метод и вернуть константу
            return new UnaryOperand(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }
    }
}