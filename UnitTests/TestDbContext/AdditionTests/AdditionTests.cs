using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests.AdditionTests
{
    [TestFixture]
    public class AdditionTests : BaseWriteTests
    {
        #region helpers

        private static IEnumerable<TestTable> DataCountGenerate(int count) =>
            Enumerable.Range(1, count).Select(TestTableGenerate).ToArray();

        private static TestTable TestTableGenerate(int i) => new TestTable
        {
            SomeULong = (ulong) i,
            SomeDate = DateTime.Now.Subtract(new TimeSpan(i)),
            SomeFloat = i,
            SomeString = $"{i}"
        };

        #endregion

        private static IEnumerable<TestTable> Data100 = DataCountGenerate(100);
        private static IEnumerable<TestTable> Data1000 = DataCountGenerate(1000);
        private static IEnumerable<TestTable> Data10000 = DataCountGenerate(10000);
        private static IEnumerable<TestTable> Data100000 = DataCountGenerate(100000);
        private static IEnumerable<TestTable> Data1000000 = DataCountGenerate(1000000);
        [Test]
        public void AddTest1000()
        {
            _context.TestTables.Add(Data1000);
            AssertCount(1000);
        }

        [Test]
        public void AddTest10000()
        {
            _context.TestTables.Add(Data10000);
            AssertCount(10000);
        }

        [Test]
        public void AddTest100000()
        {
            _context.TestTables.Add(Data100000);

            AssertCount(100000);
        }


        [Test]
        public void AddTest1000000()
        {
            _context.TestTables.Add(Data1000000);
            AssertCount(1000000);
        }
    }
}