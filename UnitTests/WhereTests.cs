using System;
using System.Linq;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests
{
    public class WhereTests
    {
        private TestDbContext _testDbContext { get; set; }

        [Test]
        public void Test0()
        {
            _testDbContext = new TestDbContext();
            var l = _testDbContext.TestTables.Where(x=>x.SomeString == "соси").ToList();
            var s = l.ToList();
        }

        [Test]
        public void Test1()
        {
            var s = 111;
            var _testDbContext = new TestDbContext();
            var l = _testDbContext.TestTables
                .Select(x => new Dto
                {
                    SomeStr = x.SomeString,
                    SomeInt1 = x.SomeInt
                })
                .Select(x => new {s = x.SomeStr})
                .Where(x => x.s == "8006-6129-3130-5580");
            var ss = l.ToList();
        }
    }
}