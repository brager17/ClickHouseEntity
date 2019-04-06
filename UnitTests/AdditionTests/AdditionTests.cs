using System;
using System.Linq;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests.AdditionTests
{
    public class AdditionTests
    {
        private TestDbContext _context { get; set; }

        [SetUp]
        public void SetUp()
        {
            _context = new TestDbContext();
        }

        [Test]
        public void AddTest()
        {
            var added = Enumerable.Range(1, 100).Select(x => new TestTable()
            {
                SomeInt = (ulong) x,
                SomeDate = DateTime.Today,
                SomeFloat = (float) x,
                SomeString = $"{x}"
            });
            
            _context.TestTables.Add(added);
        }
    }
}