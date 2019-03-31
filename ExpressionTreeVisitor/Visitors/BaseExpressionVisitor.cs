using System.Linq;
using System.Linq.Expressions;
using ClickDbContextInfrastructure;

namespace ExpressionTreeVisitor
{
    public class BaseExpressionVisitor : ExpressionVisitor
    {
        private SelectVisitor SelectVisitor { get; set; }
        private AggregateLinqInfo AggregateLinqInfo { get; set; }


        public AggregateLinqInfo GetInfo(Expression expression)
        {
            SelectVisitor = new SelectVisitor(new DtoToExpressionToLinqInfoHandler(),
                new ValueTypeExpressionToLinqInfoHandler());
            AggregateLinqInfo = new AggregateLinqInfo();
            base.Visit(expression);
            return AggregateLinqInfo;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Select") AggregateLinqInfo.SelectInfo.Add(SelectVisitor.GetInfo(node));

            return base.VisitMethodCall(node);
        }

        //todo добавить более корректную проверку, что ConstantExpression это именно ConstantExpression DbSet'а
        protected override Expression VisitConstant(ConstantExpression node)
        {
            // if node.type is DbSetType
            if (node.Type.IsGenericType && node.Type.GenericTypeArguments.Length == 1 &&
                node.Type.GenericTypeArguments.Single().IsDbSetType())
                AggregateLinqInfo.SetType = node.Type.GetGenericArguments().Single();
            return base.VisitConstant(node);
        }
    }
}