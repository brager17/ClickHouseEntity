using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;
using ClickHouseTableGenerator;
using Context;
using DbContext;
using EntityTracking;

namespace Root
{
    public static class GetClassGenerator
    {
        public static ITableGenerator Get(string connectionString, IEnumerable<IDbLogger> loggers)
        {
            var tableGenerator = new TableGenerator(
                new LoggerDecoratorWithConverter<ClassType, HasSqlStringInfo>(
                    new SqlGenerator(new EngineDbInfoFactory(), new DbTypeToCSharp(), new CreatingDbInfoToSqlInfoBy()),
                    new HasSqlStringInfoLoggerAdapter(), new[] {new FileDbLogger("../../../SQL/SQL.txt"),}),
                new WriteDbHandler<HasSqlStringInfo>(connectionString, new StubMutableQuery()));
            return tableGenerator;
        }
    }

    #region factory
    public class EngineDbInfoFactory : IQueryFactory<ClassType, IQuery<ClassType, EngineDbInfo>>
    {
        public IQuery<ClassType, EngineDbInfo> Create(ClassType input)
        {
            if (input.classType.GetCustomAttribute<DbSetSettings>() != null) return new EngineInfoBySettings();
            return new EngineInfoByAttributesName();
        }
    }
    #endregion
}