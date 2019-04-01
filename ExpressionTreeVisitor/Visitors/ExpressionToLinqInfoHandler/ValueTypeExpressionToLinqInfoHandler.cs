using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeVisitor
{
    public class ValueTypeExpressionToLinqInfoHandler : IExpressionToLinqInfoHandler<SelectInfo>
    {
        public SelectInfo GetLinqInfo(LambdaExpression expression)
        {
            var selectInfo = new SelectInfo {LambdaType = expression.Parameter().Type};
            var propertyExpressionInfos = new List<KeyValuePair<PropertyInfo, InPropertySelectExpression>>();

            if (!(expression is LambdaExpression lambdaExpression &&
                  lambdaExpression.Body is MemberExpression memberExpression))
                throw new NotSupportedException();

            var keyValuePair = new KeyValuePair<PropertyInfo, InPropertySelectExpression>(
                (PropertyInfo) memberExpression.Member, new InPropertySelectExpression(memberExpression));

            propertyExpressionInfos.Add(keyValuePair);

            propertyExpressionInfos.ForEach(x => selectInfo._propertyExpressions.Add(x.Key, x.Value));
            return selectInfo;
        }
    }
}