using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class OrderingToSqlVisitor
    {
        private  ICanGetPropertyByMemberExpression _canGetPropertyByMemberExpression { get; set; }

        public string Visit(ICanGetPropertyByMemberExpression canGetPropertyByMemberExpression, LambdaExpression lambdaExpression)
        {
            _canGetPropertyByMemberExpression = canGetPropertyByMemberExpression;
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
            return _canGetPropertyByMemberExpression.GetNameMapProperty(memberExpression);
        }

        private string MemberInitExpressionVisit(MemberInitExpression memberInitExpression)
        {
            throw new NotSupportedException();
        }

        private string NewExpressionMember(NewExpression newExpression)
        {
            var newExpressionMember = string.Join(',', newExpression.Arguments.Select(x =>
                _canGetPropertyByMemberExpression.GetNameMapProperty((MemberExpression) x)));
            return newExpressionMember;
        }
    }
}