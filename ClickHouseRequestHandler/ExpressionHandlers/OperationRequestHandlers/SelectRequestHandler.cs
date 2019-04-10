using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class SelectRequestHandler : IOperationRequestHandle<IEnumerable<SelectInfo>>
    {
        public string Handle(IEnumerable<SelectInfo> operationInfo)
        {
            var infos = operationInfo.ToList();
            // todo метод GetSourcePropertyInfo заменить на HasMapPropInfo
            var sourceDestProps = GetSourcePropertyInfo(infos);
            var selectString = string.Join(",", sourceDestProps.Select(x =>
            {
                var (source, dest) = x;
                return $"{source.GetColumnName()} AS {dest.Name}";
            }));

            return selectString;
        }

        private IEnumerable<(PropertyInfo, PropertyInfo)> GetSourcePropertyInfo(IEnumerable<SelectInfo> infos)
        {
            var selectInfos = infos.ToList();
            var keyValueItems = selectInfos.First()._propertyExpressions.Select(x =>
                new KeyValuePair<PropertyInfo, InPropertySelectExpression>(x.Key, x.Value)).ToList();
            var destProperties = keyValueItems.Select(x => x.Key);
            var names = keyValueItems.Select(x => x.Value._memberExpression.Member.Name);
            var propertiesInfos = selectInfos.Skip(1);

            foreach (var info in propertiesInfos)
            {
                var propsExpr = names.Join(info._propertyExpressions, x => x, x => x.Key.Name, (x, y) => y).ToList();
                keyValueItems = propsExpr;
                names = propsExpr.Select(x => x.Value._memberExpression.Member.Name).ToList();
            }

            var sourceProperties = keyValueItems.Select(x => (PropertyInfo) x.Value._memberExpression.Member).ToList();
            return sourceProperties.Zip(destProperties, (x, y) => (x, y));
        }
    }
}