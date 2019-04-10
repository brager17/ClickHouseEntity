using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Context;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Tests.AdditionTests;
using UnitTests.TestDbContext;

namespace Tests
{
    #region helpers

    public class SomeDto
    {
        public DateTime Date { get; set; }
        public string String { get; set; }
        public float Float { get; set; }
        public ulong Long { get; set; }
    }

    public class Dto

    {
        public string SomeStr { get; set; }
        public ulong SomeInt1 { get; set; }
    }

    #endregion helpers

    [TestFixture]
    public class TestTableTests : BaseTestTableOperations
    {
        #region helpers

        private IEnumerable<ulong> SomeULong = AddData.Data10000.Select(x => x.SomeULong);
        private IEnumerable<string> SomeString = AddData.Data10000.Select(x => x.SomeString);
        private IEnumerable<DateTime> SomeDate = AddData.Data10000.Select(x => x.SomeDate);
        private IEnumerable<float> SomeFloat = AddData.Data10000.Select(x => x.SomeFloat);

        #endregion helpers

        [SetUp]
        public void Setup()
        {
            _context.TestTables.Add(AddData.Data10000);
        }

        [Test]
        public void Test0()
        {
            var data = _context.TestTables.ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x.SomeULong)));
        }

        [Test]
        public void Test1()
        {
            var data = _context.TestTables
                .Select(x => new
                {
                    SomeInt1 = x.SomeULong,
                    SomeStr = x.SomeString
                })
                .Select(x => x.SomeInt1)
                .ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x)));
        }

        [Test]
        public void Test11()
        {
            var data = _context.TestTables
                .Select(x => x.SomeString)
                .ToList();

            AssertCount(10000);
            Assert.True(data.All(x => SomeString.Contains(x)));
        }


        [Test]
        public void Test2()
        {
            var data = _context.TestTables.Select(x => new {s = x.SomeULong, x.SomeDate}).ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x.s)));
            Assert.True(data.All(x => SomeDate.Contains(x.SomeDate)));
        }

        [Test]
        public void Test3()
        {
            var data = _context.TestTables.Select(x => new SomeDto
            {
                Long = x.SomeULong, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
            }).ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x.Long)));
            Assert.True(data.All(x => SomeDate.Contains(x.Date)));
            Assert.True(data.All(x => SomeString.Contains(x.String)));
            Assert.True(data.All(x => SomeFloat.Contains(x.Float)));
        }

        [Test]
        public void Test4()
        {
            var data = _context.TestTables.Select(x => new SomeDto
            {
                Long = x.SomeULong, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
            }).Select(x => x.Long).ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x)));
        }

        [Test]
        public void Test5()
        {
            var data = _context.TestTables.Select(x => new SomeDto
                {
                    Long = x.SomeULong, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
                }).Select(x => new {l = x.Long, f = x.Float})
                .ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x.l)));
            Assert.True(data.All(x => SomeFloat.Contains(x.f)));
        }


        [Test]
        public void Test6()
        {
            var data = _context.TestTables.Select(x => new SomeDto
                {
                    Long = x.SomeULong, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
                }).Select(x => new {l = x.Long, f = x.Float})
                .Select(x => new SomeDto()
                {
                    Long = x.l,
                    Float = x.f
                }).ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeULong.Contains(x.Long)));
            Assert.True(data.All(x => SomeFloat.Contains(x.Float)));
        }


        [Test]
        public void Test7()
        {
            var data = _context.TestTables.Select(x => new SomeDto
                {
                    Long = x.SomeULong, Date = x.SomeDate, Float = x.SomeFloat, String = x.SomeString
                }).Select(x => new {l = x.Long, f = x.Float})
                .Select(x => new SomeDto()
                {
                    Long = x.l,
                    Float = x.f
                })
                .Select(x => x.Float)
                .ToList();
            AssertCount(10000);
            Assert.True(data.All(x => SomeFloat.Contains(x)));
        }
    }
}