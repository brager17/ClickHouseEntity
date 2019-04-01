using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickDbContextInfrastructure;

namespace ExpressionTreeVisitor
{
    public enum LinqMethod
    {
        Where,
        Select,
        OrderBy
    }

    public class LinqInfo
    {
        public Expression Expression { get; set; }
        public LinqMethod LinqMethod { get; set; }
    }

    public class LinqInfoPropertiesMap
    {
        public List<SelectInfo> SelectInfos { get; set; }
        public List<LinqInfo> LinqInfoStack { get; set; }

        public LinqInfoPropertiesMap()
        {
            SelectInfos = new List<SelectInfo>();
            LinqInfoStack = new List<LinqInfo>();
        }
    }

    public class InfoVisitor
    {
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _dtoHandler;
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _simpleTypeHandler;
        private LinqInfoPropertiesMap LinqInfoPropertiesMap { get; set; }

        public InfoVisitor()
        {
            // todo при использовании заменить на инъекцию 
            _dtoHandler = new DtoToExpressionToLinqInfoHandler();
            _simpleTypeHandler = new ValueTypeExpressionToLinqInfoHandler();
            //
            LinqInfoPropertiesMap = new LinqInfoPropertiesMap();
        }

        public LinqInfoPropertiesMap Visit(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
                return VisitMethodCall(methodCallExpression);
            if (expression is ConstantExpression)
            {
                if (expression.Type.IsDbSetType())
                {
                }

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

            return Visit(expression.Arguments.First());
        }
    }


    public class BaseExpressionVisitor : ExpressionVisitor
    {
        private SelectVisitor SelectVisitor { get; set; }
        private AggregateLinqInfo AggregateLinqInfo { get; set; }
        private LinqInfoPropertiesMap LinqInfoPropertiesMap { get; set; }

        private WhereToSqlVisitor _whereToSqlVisitor { get; set; }
        private InfoVisitor _infoVisitor { get; set; }

        public AggregateLinqInfo GetInfo(Expression expression)
        {
            SelectVisitor = new SelectVisitor(new DtoToExpressionToLinqInfoHandler(),
                new ValueTypeExpressionToLinqInfoHandler());
            AggregateLinqInfo = new AggregateLinqInfo();
            _infoVisitor = new InfoVisitor();
            _whereToSqlVisitor = new WhereToSqlVisitor(new HasMapPropInfo(_infoVisitor.Visit(expression).SelectInfos));
            base.Visit(expression);
            return AggregateLinqInfo;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Select")
                AggregateLinqInfo.SelectInfo.Add(SelectVisitor.GetInfo(node.Arguments.Last()));
            if (node.Method.Name == "Where")
            {
                if (node.Arguments.Last() is UnaryExpression unary)
                {
                    AggregateLinqInfo.WhereInfo.Add(new WhereInfo()
                        {WhereStr = _whereToSqlVisitor.VisitUnaryExpression(unary)});
                }
            }

            return base.VisitMethodCall(node);
        }

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