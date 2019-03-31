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
            _testDbContext = new TestDbContext();
            var l = _testDbContext.TestTables
                .Where(x => x.SomeInt == 1 || x.SomeInt == 2 || x.SomeInt == 3 &&
                            x.SomeInt == 0 || x.SomeInt == 4 || x.SomeInt == 5)
                .Select(x => new Dto()
                {
                    SomeStr = x.SomeString,
                    SomeInt1 = x.SomeInt
                });
            l.ToList();
        }
    }
}