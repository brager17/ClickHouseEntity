using System.Linq;
using NUnit.Framework;

namespace UnitTests.TestDbContext
{
    public class YandexMetrikaTests
    {
        private YandexMetrikaDbContext _metrikaDbContext { get; set; }

        [SetUp]
        public void SetUp()
        {
            _metrikaDbContext = new YandexMetrikaDbContext();
        }

        [Test]
        public void Test0()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable.Take(10).ToList();
            Assert.True(true);
        }
    }
}