using System.Collections.Generic;
using ClickHouseTableGenerator;
using Context;

namespace UnitTests.TestDbContext
{
    public class YandexMetrikaDbContext : ClickHouseDbContext
    {
        private const string ConnectionString =
            "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=datasets";

        public YandexMetrikaDbContext() : base(ConnectionString)
        {
        }

        protected override IEnumerable<IDbLogger> _dbLoggers => new[] {new ConsoleDbLogger(),};

//        protected override IEnumerable<IDbLogger> _dbLoggers => new[] {new StubConsoleLogger(),};
        [NoCreateTable] public DbSet<YandexMetrikaTestTable> YandexMetrikaTable { get; set; }
    }
}