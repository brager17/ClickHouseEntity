using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Context;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using UnitTests.TestDbContext;

namespace Tests
{
    public class SomeDto
    {
        public DateTime Date { get; set; }
        public string String { get; set; }
        public float Float { get; set; }
        public long Long { get; set; }
    }

    #region helpers

    public static class TestHelper
    {
        public static IEnumerable<long> GetSomeInt()
        {
            return new[]
            {
                28194901262,
                28194901262,
                28194901262,
                28194901262,
                28194901262,
                28194901262,
                28194901262,
                28194901262,
            };
        }

        public static IEnumerable<string> GetSomeString()
        {
            return new[]
            {
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
                "8006-6129-3130-5580",
            };
        }


        public static IEnumerable<object> GetSomeIntSomeData()
        {
            return new[]
            {
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
                new {s = 28194901262, SomeDate = new DateTime(2010, 03, 10)},
            };
        }

        public static IEnumerable<SomeDto> SomeDtoDate()
        {
            return new[]
            {
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
                new SomeDto
                {
                    Date = new DateTime(2010, 03, 10), Float = 1169, String = "8006-6129-3130-5580",
                    Long = 28194901262
                },
            };
        }
    }

    #endregion helpers

    public class Dto
    {
        public string SomeStr { get; set; }
        public long SomeInt1 { get; set; }
    }

    public class Tests
    {
        private TestDbContext _context { get; set; }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            _context = new TestDbContext();
            var expression = _context.TestTables
                .Select(x => new
                {
                    SomeInt1 = x.SomeInt,
                    SomeStr = x.SomeString
                })
                .Select(x => x.SomeInt1);

            var s = expression.ToList();
            CollectionAssert.AreEqual(TestHelper.GetSomeInt(), expression.ToList());
        }

        [Test]
        public void Test11()
        {
            _context = new TestDbContext();
            var expression = _context.TestTables
                .Select(x => x.SomeString);
            CollectionAssert.AreEqual(TestHelper.GetSomeString(), expression.ToList());
        }


        [Test]
        public void Test2()
        {
            _context = new TestDbContext();
            var expression = _context.TestTables.Select(x => new {s = x.SomeInt, x.SomeDate}).ToList();
            CollectionAssert.AreEqual(TestHelper.GetSomeIntSomeData(), expression.ToList());
        }

        [Test]
        public void Test3()
        {
            _context = new TestDbContext();
            var expression = _context.TestTables.Select(x => new SomeDto
                {
                    Long = x.SomeInt, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
                })
                .Select(x => new
                {
                    x.Date, x.Long
                })
                .Select(x => x.Long)
                .ToList();
//            CollectionAssert.AreEqual(TestHelper.SomeDtoDate().Select(x => x.Date),
//                expression.ToList().Select(x => x.Date));
            CollectionAssert.AreEqual(TestHelper.SomeDtoDate().Select(x => x.Long),
                expression.ToList().Select(x => x));
//            CollectionAssert.AreEqual(TestHelper.SomeDtoDate().Select(x => x.Float),
//                expression.ToList().Select(x => x.Float));
//            CollectionAssert.AreEqual(TestHelper.SomeDtoDate().Select(x => x.String),
//                expression.ToList().Select(x => x.String));
        }
    }
}