using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class OrderingToSqlVisitor
    {
        private readonly IHasMapPropInfo _hasMapPropInfo;
        private IEnumerable<SelectInfo> _mapInfos { get; set; }

        public OrderingToSqlVisitor(IHasMapPropInfo hasMapPropInfo)
        {
            _hasMapPropInfo = hasMapPropInfo;
        }

        public string Visit(IEnumerable<SelectInfo> mapInfos, LambdaExpression lambdaExpression)
        {
            _mapInfos = mapInfos;
            var visitMethodCallExpressionVisit = VisitLambda(lambdaExpression);
            return visitMethodCallExpressionVisit;
        }

        public string VisitLambda(LambdaExpression lambdaExpression)
        {
            if (lambdaExpression.Body is MemberExpression memberExpression)
                return MemberExpressionVisit(memberExpression);
            if (lambdaExpression.Body is NewExpression newExpression)
                return NewExpressionMember(newExpression);

            throw new NotSupportedException();
        }

        private string MemberExpressionVisit(MemberExpression memberExpression)
        {
            return _hasMapPropInfo.GetNameMapProperty(_mapInfos, memberExpression);
        }

        private string MemberInitExpressionVisit(MemberInitExpression memberInitExpression)
        {
            throw new NotSupportedException();
        }

        private string NewExpressionMember(NewExpression newExpression)
        {
            var newExpressionMember = string.Join(',', newExpression.Arguments.Select(x =>
                _hasMapPropInfo.GetNameMapProperty(_mapInfos, (MemberExpression) x)));
            return newExpressionMember;
        }
    }
}