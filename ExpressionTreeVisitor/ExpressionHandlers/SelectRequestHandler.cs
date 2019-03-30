using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class SelectRequestHandler : IOperationRequestHandle<IEnumerable<SelectInfo>>
    {
        private readonly IGetBindingInfo _getBindingInfo;

        public SelectRequestHandler(IGetBindingInfo getBindingInfo)
        {
            _getBindingInfo = getBindingInfo;
        }

        public string Handle(IEnumerable<SelectInfo> operationInfo)
        {
            var infos = operationInfo.Reverse().ToList();
            var sourceProperties = Enumerable.Empty<PropertyInfo>();
            var sourceToDestProps = Enumerable.Empty<(PropertyInfo, PropertyInfo)>();
            if (infos.Last().LambdaType.IsClass)
            {
                var destProperties = infos.Last()._propertyExpressions.Select(x => x.Key);
                if (infos.Count == 1)
                    sourceProperties = infos
                        .Last()._propertyExpressions
                        .Select(x => (PropertyInfo) x.Value._memberExpression.Member);
                else sourceProperties = GetSourcePropertyInfo(destProperties, infos.SkipLast(1));
                sourceToDestProps = sourceProperties.Zip(destProperties, (x, y) => (x, y));
            }

            else if (infos.Last().IsPrimitiveSelect)
            {
                var destProperties = new[] {infos.Last()._propertyExpressions.Single().Key};
                if (infos.Count == 1)
                    sourceProperties = infos.Last()._propertyExpressions
                        .Select(x => (PropertyInfo) x.Value._memberExpression.Member);
                else sourceProperties = GetSourcePropertyInfo(destProperties, infos.SkipLast(1));
                sourceToDestProps = sourceProperties.Zip(destProperties, (x, y) => (x, y));
            }

            var selectString = string.Join(",", sourceToDestProps.Select(x =>
            {
                var (source, dest) = x;
                return $"{source.GetColumnName()} AS {dest.Name}";
            }));

            return "SELECT " + selectString + " FROM ";
        }

        //todo дублирование GetBindingInfo, нужно убрать когда сложится картина как биндить объекты
        private IEnumerable<PropertyInfo> GetSourcePropertyInfo(IEnumerable<PropertyInfo> destProps,
            IEnumerable<SelectInfo> infos)
        {
            var names = destProps.Select(x => x.Name).ToList();
            var props = new List<PropertyInfo>();
            if (!infos.Any()) return destProps;
            foreach (var info in infos)
            {
                var join = names.Join(info._propertyExpressions, x => x, x => x.Key.Name, (x, y) => (x, y));
                names = join.Select(x => x.Item2.Key.Name).ToList();
                props = join.Select(x => (PropertyInfo) x.Item2.Value._memberExpression.Member).ToList();
            }

            return props;
        }
    }
}