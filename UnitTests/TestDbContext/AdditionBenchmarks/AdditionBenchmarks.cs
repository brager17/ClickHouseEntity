using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using NUnit.Framework.Internal;

namespace UnitTests.TestDbContext.AdditionBenchmarks
{
    public class AdditionBenchmarks
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

        private TestDbContext _context = new TestDbContext();

        private static IEnumerable<TestTable> Data100 = DataCountGenerate(100);
        private static IEnumerable<TestTable> Data1000 = DataCountGenerate(1000);
        private static IEnumerable<TestTable> Data10000 = DataCountGenerate(10000);
        private static IEnumerable<TestTable> Data100000 = DataCountGenerate(100000);
     


        [Benchmark]
        public void Add100Rows()
        {
            _context.TestTables.Add(Data100);
        }

        [Benchmark]
        public void Add1000Rows()
        {
            _context.TestTables.Add(Data1000);
        }


        [Benchmark]
        public void Add10000Rows()
        {
            var data = Data1000;
            _context.TestTables.Add(Data10000);
        }


//        [Benchmark]
        public void Add100000Rows()
        {
            _context.TestTables.Add(Data100000);
        }
    }
}