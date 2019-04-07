using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouse.Ado;
using NUnit.Framework;

namespace UnitTests.TestDbContext
{
    #region helpers

    public class SelectYandexMetrikaTestDto
    {
        public ulong watchID { get; set; }
        public int javaEnable { get; set; }
        public string title { get; set; }
        public int goodEvent { get; set; }
        public DateTime eventTime { get; set; }
    }

    public class SelectYandexMetrikaTestTitleDto
    {
        public string t { get; set; }
        public DateTime e { get; set; }
        public int je { get; set; }
    }

    #endregion helpers

    public class YandexMetrikaTests
    {
        #region helpers

        private YandexMetrikaDbContext _metrikaDbContext { get; set; }

        private IQueryable<SelectYandexMetrikaTestDto> BaseSelect => _metrikaDbContext.YandexMetrikaTable
            .Select(x => new SelectYandexMetrikaTestDto()
            {
                title = x.Title,
                eventTime = x.EventTime,
                goodEvent = x.GoodEvent,
                javaEnable = x.JavaEnable,
                watchID = x.WatchID
            });

        #endregion helpers

        [SetUp]
        public void SetUp()
        {
            _metrikaDbContext = new YandexMetrikaDbContext();
        }

        [Test]
        public void TakeTest()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable
                .Take(10)
                .ToList();
            Assert.AreEqual(10, allMetrika.Count());
        }

        [Test]
        public void SelectTakeTest()
        {
            var allMetrika = BaseSelect
                .Take(10)
                .ToList();

            Assert.AreEqual(10, allMetrika.Count);
        }

        [Test]
        public void SelectWhereTakeTest()
        {
            var allMetrika = BaseSelect
                .Take(10)
                .ToList();
            Assert.AreEqual(10, allMetrika.Count);
        }

        [Test]
        public void SelectWhereSimpleSelectTakeTest()
        {
            var data = BaseSelect
                .Where(x => x.title != "")
                .Select(x => x.title)
                .Take(10)
                .ToList();
            Assert.AreEqual(10, data.Count);
        }

        [Test]
        public void SelectWhereAnonymouseSelectTakeTest()
        {
            var data = BaseSelect.Where(x => x.title != "" && x.javaEnable == 0)
                .Select(x => new {x.title, x.javaEnable})
                .Take(10)
                .ToList();

            Assert.True(data.Any(x => x.title != "" && x.javaEnable == 0));
            Assert.AreEqual(10, data.Count);
        }


        [Test]
        public void SelectWhereConcreteSelectTakeTest()
        {
            var data = BaseSelect.Where(x => x.title != "" && x.javaEnable == 0)
                .Select(x => new SelectYandexMetrikaTestTitleDto()
                {
                    t = x.title,
                    e = x.eventTime,
                    je = x.javaEnable
                })
                .Take(10)
                .ToList();

            Assert.True(data.Any(x => x.t != "" && x.je == 0));
            Assert.AreEqual(10, data.Count);
        }

        [Test]
        public void WhereTakeTest()
        {
            var data = _metrikaDbContext.YandexMetrikaTable
                .Where(x => x.Title != "" && x.JavaEnable == 0)
                .Take(10)
                .ToList();
            Assert.True(data.Any(x => x.Title != "" && x.JavaEnable == 0));
            Assert.AreEqual(10, data.Count);
        }

        [Test]
        public void OrderByTest()
        {
            var data = _metrikaDbContext.YandexMetrikaTable
                .Take(10)
                .OrderBy(x => new {x.Age, x.Sex}).ToList();

            var lastItem = data.Last();
            Assert.True(data.TrueForAll(x => x.Age <= lastItem.Age));
            Assert.True(data.TrueForAll(x => x.Sex <= lastItem.Sex));
            Assert.AreEqual(10, data.Count);
        }

        [Test]
        public void ArraySelectTest()
        {
            //ADONet ClickHouse хреново написали свой провайдер,
            //поэтому Take доверять незья, он может вернуть меньшее коичество записей
            var all = _metrikaDbContext.YandexMetrikaTable.Select(x => x.RefererRegions).Take(20).ToList();
        }
    }
}