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
                .Select(x => new Dto()
                {
                    SomeStr = x.SomeString,
                    SomeInt1 = x.SomeInt
                })
                .Where(x => x.SomeInt1 == 28194901262 && x.SomeStr == "8006-6129-3130-5580");
            var ss = l.ToList();
        }
    }
}