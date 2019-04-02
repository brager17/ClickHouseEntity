using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DbContext;

namespace ExpressionTreeVisitor
{
    public interface IHasMapPropInfo
    {
        PropertyInfo GetSourcePropInfo(IEnumerable<SelectInfo> infos, MemberExpression info);
        string GetNameMapProperty(IEnumerable<SelectInfo> infos, MemberExpression memberExpression);
    }

    public class HasMapPropInfo : IHasMapPropInfo
    {
        public PropertyInfo GetSourcePropInfo(IEnumerable<SelectInfo> infos, MemberExpression memberExpression)
        {
            var selectInfo = infos.Select((x, i) => new
                {
                    isFlag = x._propertyExpressions.Any(xx => xx.Key.Name == memberExpression.Member.Name),
                    counter = i + 1,
                    prop = x._propertyExpressions.SingleOrDefault(xx => xx.Key.Name == memberExpression.Member.Name)
                })
                .First(x => x.isFlag);

            return Upstairs(selectInfo.prop, infos.Skip(selectInfo.counter));
        }


        PropertyInfo Upstairs(
            KeyValuePair<PropertyInfo, InPropertySelectExpression> propInfo,
            IEnumerable<SelectInfo> upstairsSelects)
        {
            var memberProp = upstairsSelects.Aggregate(propInfo.Value._memberExpression,
                (current, selectInfo) => selectInfo._propertyExpressions.Single(x => x.Key.Name == current.Member.Name)
                    .Value._memberExpression);

            return (PropertyInfo) memberProp.Member;
        }

        public string GetNameMapProperty(IEnumerable<SelectInfo> infos, MemberExpression memberExpression) =>
            GetSourcePropInfo(infos, memberExpression).GetColumnName();
    }
}