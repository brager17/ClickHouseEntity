using System;
using System.Collections.Generic;
using ClickHouseTableGenerator;
using Context;
using DbContext;

namespace UnitTests.TestDbContext
{
    [TableName("mytable")]
    public class TestTable
    {
        [ColumnKey("some_date")]
        [PartitionKey]
        public DateTime SomeDate { get; set; }

        [ColumnKey("some_int")] [OrderKey] public ulong SomeULong { get; set; }
        [ColumnKey("some_str")] [OrderKey] public string SomeString { get; set; }
        [ColumnKey("some_float")] public float SomeFloat { get; set; }
    }


    public class TestDbContext : ClickHouseDbContext
    {
        private const string ConnectionString =
            "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=test";

        public TestDbContext() : base(ConnectionString)
        {
        }

        protected override IEnumerable<IDbLogger> _dbLoggers => new[] {new ConsoleDbLogger(),};

        public DbSet<TestTable> TestTables { get; set; }
    }
}