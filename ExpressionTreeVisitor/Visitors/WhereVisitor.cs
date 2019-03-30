using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class WhereVisitor : ExpressionVisitor, IGetInfo<WhereInfo>
    {
        private WhereInfo WhereInfo { get; set; }

        public WhereInfo GetInfo(Expression expression)
        {
            WhereInfo = new WhereInfo();
            base.Visit(expression);
            return WhereInfo;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }
    }
}