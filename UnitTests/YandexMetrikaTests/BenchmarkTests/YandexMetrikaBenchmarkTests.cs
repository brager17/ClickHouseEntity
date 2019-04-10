using System.Linq;
using BenchmarkDotNet.Attributes;
using ClickHouse.Ado;
using NUnit.Framework;

namespace UnitTests.TestDbContext.BenchmarkTests
{
    public class YandexMetrikaBenchmarkTests
    {
        private YandexMetrikaDbContext _metrikaDbContext { get; set; }
        private AdoBenchMarkSelectOperation adoOperation { get; set; }

        public YandexMetrikaBenchmarkTests()
        {
            _metrikaDbContext = new YandexMetrikaDbContext();
            adoOperation = new AdoBenchMarkSelectOperation();
        }

        /*[Params(100, 1000)] */
        public int TakeAllSelect { get; set; }

        /*[Params(100, 1000, 10000)] */
        public int TakeSelectInDto { get; set; }


        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
//        [Benchmark]
        public void SelectAll(int count)
        {
            var list = _metrikaDbContext.YandexMetrikaTable.Take(count).ToList();
        }

        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
//        [Benchmark]
        public void AdoNetGetAll(int count)
        {
            adoOperation.Get("SELECT * FROM hits_v1 LIMIT " + count);
        }

//        [Arguments(10)]
//        [Arguments(100)]
//        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Arguments(1000000)]
//        [Benchmark]
        public void SelectInDtoWith5Properties(int count)
        {
            var list = _metrikaDbContext.YandexMetrikaTable.Select(x => new SelectYandexMetrikaTestDto
            {
                title = x.Title, eventTime = x.EventDate, goodEvent = x.GoodEvent, javaEnable = x.JavaEnable,
                watchID = x.WatchID
            }).Take(count).ToList();
        }

        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Benchmark]
        public void SelectInAnonymouseClass5Properties(int count)
        {
            var list = _metrikaDbContext.YandexMetrikaTable.Select(x => new
            {
                x.Title,
                x.EventDate,
                x.GoodEvent,
                x.JavaEnable,
                x.WatchID
            }).Take(count).ToList();
        }

        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Benchmark]
        public void AdoNetSelectFiveColums(int count)
        {
            adoOperation.Get("SELECT Title,EventDate,GoodEvent,JavaEnable,WatchID FROM hits_v1 LIMIT " + count);
        }

        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
//        [Benchmark]
        public void SelectInPrimitiveType(int count)
        {
            var s = _metrikaDbContext.YandexMetrikaTable.Select(x => x.Title).Take(count).ToList();
        }

        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
//        [Benchmark]
        public void AdoNetTestSelectInPrimitiveType(int count)
        {
            adoOperation.Get("SELECT Title FROM hits_v1 LIMIT " + count);
        }


        private class AdoBenchMarkSelectOperation
        {
            private string ConnectionString =>
                "Host=localhost;Port=9000;User=default;Password=;SocketTimeout=600000;Database=datasets";

            public void Get(string sql)
            {
                var settings = new ClickHouseConnectionSettings(ConnectionString);
                using (var cnn = new ClickHouseConnection(settings))
                {
                    cnn.Open();
                    using (var reader = cnn.CreateCommand(sql).ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                            }
                        } while (reader.NextResult());
                    }
                }
            }
        }
    }
}