using System;
using System.Linq;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseTableGenerator;
using DbContext;
using ExpressionTreeVisitor;

namespace Root
{
    public class EngineInfoByAttributesName : IQuery<ClassType, EngineDbInfo>
    {
        public EngineDbInfo Query(ClassType input)
        {
            var tableClassType = input.classType;
            var granularity = tableClassType.GetNameAttributeValueEnumMember<int>();
            // todo нужно общее решение для таких случаев
            // значение по умолчанию для granularity 8172, но если пользователь вообще не проставит это
            // то мы не сможем получить это значение
            if (granularity == default(int)) granularity = new IndexGranularity().Key;
            var order = tableClassType.GetColumnNameByAttributeOnPropertyInfo<OrderKey>().ToList();
            var partition = tableClassType.GetColumnNameByAttributeOnPropertyInfo<PartitionKey>().ToList();
            var samples = tableClassType.GetColumnNameByAttributeOnPropertyInfo<SampleAttribute>().ToList();

            ///обязательные атрибуты
            if (!order.Any()) throw new ArgumentNullException("Не удается найти атрибут OrderKey");
            if (!partition.Any()) throw new ArgumentNullException("Не удается найти атрибут PartitionKey");
            //
            return new EngineDbInfo
            {
                IndexGranularity = granularity,
                OrderKeys = order,
                PartitionKeys = partition,
                Samples = samples
            };
        }
    }
}