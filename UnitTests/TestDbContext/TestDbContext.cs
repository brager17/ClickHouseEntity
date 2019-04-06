using System;
using Context;
using DbContext;

namespace UnitTests.TestDbContext
{
    [TableName("mytable")]
    public class TestTable
    {
        [ColumnName("some_date")] public DateTime SomeDate { get; set; }
        [ColumnName("some_int")] public ulong SomeInt { get; set; }
        [ColumnName("some_str")] public string SomeString { get; set; }
        [ColumnName("some_float")] public float SomeFloat { get; set; }
    }


    public class TestDbContext : ClickHouseDbContext
    {
        private const string ConnectionString =
            "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=test";

        public TestDbContext() : base(ConnectionString)
        {
        }

        public DbSet<TestTable> TestTables { get; set; }
    }
}