using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;
using ClickHouseTableGenerator;
using Context;
using DbContext;
using EntityTracking;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetClassGenerator
    {
        public static ITableGenerator Get(string connectionString, IEnumerable<IDbLogger> loggers)
        {
            var tableGenerator = new TableGenerator(
                new LoggerDecoratorWithConverter<ClassType, HasSqlStringInfo>(
                    new SqlGenerator(new EngineDbInfoFactory(), new DbTypeToCSharp(), new CreatingDbInfoToSqlInfoBy()),
                    new HasSqlStringInfoLoggerAdapter(), new []{new FileDbLogger("../../../SQL/1.txt"), }),
                new WriteDbHandler<HasSqlStringInfo>(connectionString, new StubMutableQuery()));
            return tableGenerator;
        }
    }

    public class EngineDbInfoFactory : IQueryFactory<ClassType, IQuery<ClassType, EngineDbInfo>>
    {
        public IQuery<ClassType, EngineDbInfo> Create(ClassType input)
        {
            if (input.classType.GetCustomAttribute<DbSetSettings>() != null) return new EngineInfoBySettings();
            return new EngineInfoByAttributesName();
        }
    }

    public class EngineInfoByAttributesName : IQuery<ClassType, EngineDbInfo>
    {
        public EngineDbInfo Query(ClassType input)
        {
            var tableClassType = input.classType;
            var granularity = tableClassType.GetClassAttributeKey<int>();
            // todo нужно общее решение для таких случаев
            // значение по умолчанию для granularity 8172, но если пользователь вообще не проставит это
            // то мы не сможем получить это значение
            if (granularity == default(int)) granularity = new IndexGranularity().Key;
            var order = tableClassType.GetColumnNameByAttributeOnProperty<OrderKey>().ToList();
            var partition = tableClassType.GetColumnNameByAttributeOnProperty<PartitionKey>().ToList();
            var samples = tableClassType.GetColumnNameByAttributeOnProperty<SampleAttribute>().ToList();

            if (!order.Any()) throw new ArgumentNullException("Не удается найти атрибут OrderKey");
            if (!partition.Any()) throw new ArgumentNullException("Не удается найти атрибут PartitionKey");
            return new EngineDbInfo
            {
                IndexGranularity = granularity,
                OrderKeys = order,
                PartitionKeys = partition,
                Samples = samples
            };
        }
    }

    public class EngineInfoBySettings : IQuery<ClassType, EngineDbInfo>
    {
        public EngineDbInfo Query(ClassType input) => input.classType.GetCustomAttribute<DbSetSettings>().Key.Create();
    }
}