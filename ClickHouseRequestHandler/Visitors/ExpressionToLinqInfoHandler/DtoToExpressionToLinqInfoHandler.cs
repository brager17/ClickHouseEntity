using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeVisitor
{
    public class DtoToExpressionToLinqInfoHandler : IExpressionToLinqInfoHandler<SelectInfo>
    {
        public SelectInfo GetLinqInfo(LambdaExpression expression)
        {
            var selectInfo = new SelectInfo {LambdaType = expression.Parameter().Type};
            var list = new List<KeyValuePair<PropertyInfo, InPropertySelectExpression>>();

            KeyValuePair<PropertyInfo, InPropertySelectExpression> keyValue;

            if (expression.Body is NewExpression newExpression)
            {
                var props = newExpression.Members.Cast<PropertyInfo>();
                var ctorMembersExpressions = newExpression.Arguments.ToList().Cast<MemberExpression>();
                var propsMemberZip = props.Zip(ctorMembersExpressions,
                    (x, y) => new {anonymPropType = x, memberExpression = y}).ToList();

                list.AddRange(propsMemberZip.Select(x => CreateKeyValuePair(x.anonymPropType,
                    new InPropertySelectExpression(x.memberExpression))));
            }
            else if (expression.Body is MemberInitExpression memberInitExpression)
                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    if (!(memberBinding is MemberAssignment assignment &&
                          assignment.Expression is MemberExpression memberExpression))
                        throw new NotSupportedException();

                    keyValue = CreateKeyValuePair((PropertyInfo) assignment.Member,
                        new InPropertySelectExpression(memberExpression));
                    list.Add(keyValue);
                }
            else throw new NotSupportedException();

            list.ForEach(x => selectInfo._propertyExpressions.Add(x.Key, x.Value));
            return selectInfo;
        }

        private KeyValuePair<PropertyInfo, InPropertySelectExpression> CreateKeyValuePair
            (PropertyInfo info, InPropertySelectExpression selectExpression) =>
            new KeyValuePair<PropertyInfo, InPropertySelectExpression>(info, selectExpression);
    }
}