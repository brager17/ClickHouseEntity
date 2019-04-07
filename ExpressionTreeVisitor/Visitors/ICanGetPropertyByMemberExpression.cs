using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;

namespace ExpressionTreeVisitor
{
    public interface ICanGetPropertyByMemberExpression
    {
        PropertyInfo GetSourcePropInfo(MemberExpression info);
        string GetNameMapProperty(MemberExpression memberExpression);
    }

    public class GetPropByMember : ICanGetPropertyByMemberExpression
    {
        private readonly IEnumerable<SelectInfo> infos;

        public GetPropByMember(IEnumerable<SelectInfo> selectInfos)
        {
            infos = selectInfos;
        }

        public PropertyInfo GetSourcePropInfo(MemberExpression memberExpression)
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

        public string GetNameMapProperty(MemberExpression memberExpression) =>
            GetSourcePropInfo(memberExpression).GetColumnName();
    }

    public class GetPropsByMemberFactory : IQueryFactory<IEnumerable<SelectInfo>, ICanGetPropertyByMemberExpression>
    {
        public ICanGetPropertyByMemberExpression Create(IEnumerable<SelectInfo> input) => new GetPropByMember(input);
    }
}