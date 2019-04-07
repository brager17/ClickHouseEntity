using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTests.TestDbContext.DeleteTests
{
    [TestFixture]
    public class DeleteTests:BaseTestTableOperations
    {
        private TestDbContext _context;

        #region helpers

        private static IEnumerable<TestTable> FirstPartMockData = Enumerable.Range(1, 1000).Select(x => new TestTable()
        {
            SomeDate = new DateTime(1, 1, 1),
            SomeFloat = x,
            SomeString = "1",
            SomeULong = (ulong) x
        });

        private static IEnumerable<TestTable> SecondPartMockData = Enumerable.Range(1000, 1000).Select(x =>
            new TestTable()
            {
                SomeDate = new DateTime(2, 2, 2),
                SomeFloat = x,
                SomeString = "2",
                SomeULong = (ulong) x
            });
        public void RemoveBy(Expression<Func<TestTable, bool>> filter)
        {
            _context.TestTables.Remove(filter);
        }

        private void AddMockData()
        {
            _context.TestTables.Add(FirstPartMockData.Concat(SecondPartMockData));
        }

        #endregion helpers

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _context = new TestDbContext();
            ClearTestTable();
        }


        [SetUp]
        public void Setup()
        {
            AddMockData();
        }

        [TearDown]
        public void TearDown()
        {
            ClearTestTable();
        }

        [Test]
        public void DeleteBySomeFloatNoEquals1()
        {
            RemoveBy(x => x.SomeFloat != 1);
            AssertCount(1);
        }

        [Test]
        public void DeleteBySomeFloatEquals1()
        {
            RemoveBy(x => x.SomeFloat == 1);
            AssertCount(1999);
        }

        [Test]
        public void DeleteBySomeStringEquals1()
        {
            RemoveBy(x => x.SomeString == "1");
            AssertCount(1000);
        }

        [Test]
        public void DeleteBySomeStringEquals2()
        {
            RemoveBy(x => x.SomeString == "2");
            AssertCount(1000);
        }

        [Test]
        public void DeleteBySomeFloatStringEquals1OrEquals2()
        {
            RemoveBy(x => x.SomeString == "1" || x.SomeString == "2");
            AssertCount(0);
        }
    }
}