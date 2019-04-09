using System.Collections.Generic;
using System.ComponentModel;
using ClickHouseDbContextExntensions.CQRS;

namespace ClickHouseTableGenerator
{
    /// <summary>
    /// Базовая информация о движке Clickhouse
    /// </summary>
    public class EngineDbInfo
    {
        public int IndexGranularity { get; set; }
        public IEnumerable<string> PartitionKeys { get; set; }
        public IEnumerable<string> OrderKeys { get; set; }
        public IEnumerable<string> Samples { get; set; }

        public EngineDbInfo()
        {
            IndexGranularity = 8172;
        }
    }

    public class CreateDbInfo
    {
        public  IEnumerable<DBColumn> DbColumns { get; set; }
        public  string TableName { get; set; }
        public EngineDbInfo EngineDbInfo { get; set; }
    }
}