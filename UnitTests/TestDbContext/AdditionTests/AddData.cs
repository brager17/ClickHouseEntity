using System;
using System.Collections.Generic;
using System.Linq;
using UnitTests.TestDbContext;

namespace Tests.AdditionTests
{
    public static class AddData
    {
        public static IEnumerable<TestTable> DataCountGenerate(int count) =>
            Enumerable.Range(1, count).Select(TestTableGenerate).ToArray();

        private static TestTable TestTableGenerate(int i) => new TestTable
        {
            SomeULong = (ulong) i,
            SomeDate = new DateTime(1, 1, 1),
            SomeFloat = i,
            SomeString = $"{i}"
        };

        public static IEnumerable<TestTable> Data100 = DataCountGenerate(100);
        public static IEnumerable<TestTable> Data1000 = DataCountGenerate(1000);
        public static IEnumerable<TestTable> Data10000 = DataCountGenerate(10000);
        public static IEnumerable<TestTable> Data100000 = DataCountGenerate(100000);
        public static IEnumerable<TestTable> Data1000000 = DataCountGenerate(1000000);
    }
}