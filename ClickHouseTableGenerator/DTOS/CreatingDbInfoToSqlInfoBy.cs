using System.Linq;
using System.Xml.Linq;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;

namespace ClickHouseTableGenerator
{
    public class CreatingDbInfoToSqlInfoBy : IQuery<CreatingDbInfo, HasSqlStringInfo>
    {
        public HasSqlStringInfo Query(CreatingDbInfo input)
        {
            var partitions = $"({string.Join(',', input.EngineDbInfo.PartitionKeys)})";
            var orderKeys = $"({string.Join(',', input.EngineDbInfo.OrderKeys)})";
            var initializers = string.Join(",\n", input.DbColumns.Select(x => $"{x.Name}   {x.DbTypeName}"));
            var sqlInfo = new CreateTableSqlInfo
            {
                Sql = $"CREATE TABLE IF NOT EXISTS {input.TableName} \n({initializers}) \n" +
                      "ENGINE = MergeTree()\n" +
                      $"PARTITION BY {partitions}\n" +
                      $"ORDER BY {orderKeys}\n"
            };
            if (input.EngineDbInfo.Samples.Any())
                sqlInfo.Sql += $"SAMPLE BY {string.Join(',', input.EngineDbInfo.Samples)}\n";
            sqlInfo.Sql += $"SETTINGS index_granularity = {input.EngineDbInfo.IndexGranularity};";
            return sqlInfo;
        }
    }
}