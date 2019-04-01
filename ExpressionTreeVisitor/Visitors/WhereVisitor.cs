using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DbContext;

namespace ExpressionTreeVisitor
{
    public class WhereToSqlVisitor
    {
        private readonly IHasMapPropInfo _hasMapPropInfo;

        public string VisitUnaryExpression(UnaryExpression expression)
        {
            if (expression.Operand is MemberExpression member && member.Expression is ConstantExpression constant)
                return $"{GetValueFromFieldInAutoGenerateClass(expression)}";

            return VisitLambda((LambdaExpression) expression.Operand);
        }

        public WhereToSqlVisitor(IHasMapPropInfo hasMapPropInfo)
        {
            _hasMapPropInfo = hasMapPropInfo;
        }

        public string VisitExpression(Expression expression)
        {
            return VisitLambda((LambdaExpression) expression);
        }

        public string VisitLambda(LambdaExpression node)
        {
            if (node.Body is BinaryExpression binaryExpression)
                return VisitBinaryExpression(binaryExpression);
            throw new NotSupportedException();
        }

        public string VisitBinaryExpression(BinaryExpression expression)
        {
            var left = LeftAndRightVisitBinaryExpression(expression.Left);
            var right = LeftAndRightVisitBinaryExpression(expression.Right);
            var @operator = VisitExpressionType(expression.NodeType);
            return $"{left} {@operator} {right}";
        }

        private string LeftAndRightVisitBinaryExpression(Expression expression)
        {
            var memberSql = string.Empty;
            if (expression is MemberExpression memberExpression)
                memberSql = VisitMemberExpression(memberExpression);
            if (expression is BinaryExpression binaryExpression)
                memberSql = VisitBinaryExpression(binaryExpression);
            if (expression is ConstantExpression constantExpression)
                memberSql = VisitExpressionConstant(constantExpression);
            if (expression is UnaryExpression unaryExpression)
                memberSql = VisitUnaryExpression(unaryExpression);
            if (expression is MethodCallExpression methodCallExpression)
                throw new NotImplementedException();
            return memberSql;
        }


        private string VisitMemberExpression(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression)
                return $"{GetValueFromFieldInAutoGenerateClass(memberExpression)}";

            return $"{GetNameMapProperty(memberExpression)}";
        }

        //todo refactoring сделать нормальное сравнение типов
        //todo заменить компиляцию на поиск значения в автосгенерированном классе
        private object GetValueFromFieldInAutoGenerateClass(Expression expression)
        {
            if (expression.Type == typeof(string))
                return $"\'{Expression.Lambda(expression).Compile().DynamicInvoke()}\'";
            else if (new Type[] {typeof(int), typeof(long), typeof(float), typeof(bool), typeof(double)}.Contains(
                expression.Type))
                return $"{Expression.Lambda(expression).Compile().DynamicInvoke()}";
            else throw new NotSupportedException();
        }

        public string VisitExpressionType(ExpressionType type)
        {
            if (type == ExpressionType.AndAlso) return "AND";
            if (type == ExpressionType.OrElse) return ("OR");
            if (type == ExpressionType.Equal) return ("=");
            if (type == ExpressionType.GreaterThan) return (">");
            if (type == ExpressionType.LessThan) return ("<");
            if (type == ExpressionType.NotEqual) return ("!=");

            throw new NotImplementedException();
        }

        public string VisitExpressionConstant(ConstantExpression expression)
        {
            return expression.ToString();
            return string.Empty;
        }

        public string VisitMethodCallExpression(MethodCallExpression expression)
        {
            throw new NotImplementedException();
        }


        private string GetNameMapProperty(MemberExpression memberExpression) =>
            _hasMapPropInfo.GetSourcePropInfo(memberExpression).GetColumnName();
    }
}