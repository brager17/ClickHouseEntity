using System;
using System.Linq;
using System.Linq.Expressions;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;

namespace ExpressionTreeVisitor
{
    public class PropertyMapInfoVisitor : IQuery<Expression, LinqInfoPropertiesMap>
    {
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _dtoHandler;
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _simpleTypeHandler;
        private LinqInfoPropertiesMap LinqInfoPropertiesMap { get; set; }

        public PropertyMapInfoVisitor(
            IExpressionToLinqInfoHandler<SelectInfo> dtoWalker,
            IExpressionToLinqInfoHandler<SelectInfo> primitiveTypeWalker)
        {
            _dtoHandler = dtoWalker;
            _simpleTypeHandler = primitiveTypeWalker;
        }

        public LinqInfoPropertiesMap Query(Expression expression)
        {
            LinqInfoPropertiesMap = new LinqInfoPropertiesMap();
            return Visit(expression);
        }

        private LinqInfoPropertiesMap Visit(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
                return VisitMethodCall(methodCallExpression);
            if (expression is ConstantExpression)
            {
                return LinqInfoPropertiesMap;
            }

            if (expression is LambdaExpression lambdaExpression)
                return VisitLambda(lambdaExpression);
            throw new NotSupportedException();
        }

        private LinqInfoPropertiesMap VisitLambda(LambdaExpression expression)
        {
            SelectInfo info = null;
            if (expression.Body is NewExpression || expression.Body is MemberInitExpression)
                info = _dtoHandler.GetLinqInfo(expression);
            if (expression.Body is MemberExpression)
                info = _simpleTypeHandler.GetLinqInfo(expression);

            LinqInfoPropertiesMap.SelectInfos.Add(info);
            return LinqInfoPropertiesMap;
        }

        private LinqInfoPropertiesMap VisitMethodCall(MethodCallExpression expression)
        {
            var innerLambda = expression.Arguments.Last();
            var linqMethod = default(LinqMethod);
            if (new[] {"Select", "Where"}.Contains(expression.Method.Name))
            {
                if (expression.Method.Name == "Select")
                {
                    linqMethod = LinqMethod.Select;
                    if (innerLambda is UnaryExpression unary && unary.Operand is LambdaExpression lambdaExpression)
                        VisitLambda(lambdaExpression);
                }
                else if (expression.Method.Name == "Where")
                    linqMethod = LinqMethod.Where;

                LinqInfoPropertiesMap.LinqInfoStack.Add(new LinqInfo
                {
                    Expression = innerLambda,
                    LinqMethod = linqMethod
                });
            }

            return Visit(expression.Arguments.First());
        }
    }
}