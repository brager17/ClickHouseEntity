using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests.AdditionTests
{
    public class AdditionTests
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
        private TestDbContext _context { get; set; }

        [SetUp]
        public void SetUp()
        {
            _context = new TestDbContext();
        }

        [Test]
        public void AddTest1000()
        {
            _context.TestTables.Add(Data1000);
          
        }

        [Test]
        public void AddTest10000()
        {
            _context.TestTables.Add(Data10000);
        }

        [Test]
        public void AddTest100000()
        {
            var added = Data100000;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _context.TestTables.Add(added);
            Console.WriteLine();
            Console.WriteLine(stopWatch.Elapsed);
            Console.WriteLine();
        }


        [Test]
        public void AddTest1000000()
        {
            _context.TestTables.Add(Data1000000);
        }
    }
}