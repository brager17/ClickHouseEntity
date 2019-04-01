using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeVisitor
{
    public interface IHasMapPropInfo
    {
        PropertyInfo GetSourcePropInfo(MemberExpression info);
    }

    class HasMapPropInfo : IHasMapPropInfo
    {
        public IEnumerable<SelectInfo> Infos { get; }

        public HasMapPropInfo(IEnumerable<SelectInfo> infos)
        {
            Infos = infos;
        }

        public PropertyInfo GetSourcePropInfo(MemberExpression memberExpression)
        {
            var ss = Infos.Select((x, i) => new
            {
                isFlag = x._propertyExpressions.Any(xx => xx.Key.Name == memberExpression.Member.Name),
                counter = i,
                prop = x._propertyExpressions.Where(xx => xx.Key.Name == memberExpression.Member.Name)
            });

            var ssss = Infos.Select((x, i) => new
            {
                isFlag = x._propertyExpressions.Any(xx => xx.Key.Name == memberExpression.Member.Name),
                counter = i,
                prop = x._propertyExpressions.Where(xx => xx.Key.Name == memberExpression.Member.Name)
            });
            var selectInfo = Infos.Select((x, i) => new
                {
                    isFlag = x._propertyExpressions.Any(xx => xx.Key.Name == memberExpression.Member.Name),
                    counter = i,
                    prop = x._propertyExpressions.Single(xx => xx.Key.Name == memberExpression.Member.Name)

                })
                .Single(x => x.isFlag);

            return Upstairs(selectInfo.prop, Infos.Take(selectInfo.counter));
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
    }
}