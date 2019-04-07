using System;
using System.Linq;
using NUnit.Framework;

namespace UnitTests.TestDbContext
{
    public class BaseWriteTests
    {
        protected TestDbContext _context { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _context = new TestDbContext();
            ClearTestTable();
        }

        [TearDown]
        public void TearDown()
        {
            ClearTestTable();
        }

        protected void AssertCount(int count)
        {
            Assert.AreEqual(count, _context.TestTables.ToList().Count);
        }
        protected void ClearTestTable()
        {
            do
            {
                var randomStr = new Random().Next(1, 1000000000).ToString();
                _context.TestTables.Remove(x => x.SomeString != randomStr);
            } while (_context.TestTables.ToList().Count != 0);
        }
    }
}