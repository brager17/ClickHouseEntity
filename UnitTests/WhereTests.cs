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
        public void Test1()
        {
            var s = 111;
            var sss = Enumerable.Range(1, 100).Aggregate(string.Empty, (a, c) => a.ToString() + " " + c.ToString());
            _testDbContext = new TestDbContext();
            var l = _testDbContext.TestTables
                .Where(x => x.SomeInt == 1 || x.SomeInt == s || x.SomeInt == 3 &&
                            x.SomeInt == 0 || x.SomeString == sss || x.SomeInt == 5)
                .Select(x => new Dto()
                {
                    SomeStr = x.SomeString,
                    SomeInt1 = x.SomeInt
                });
            l.ToList();
        }
    }
}