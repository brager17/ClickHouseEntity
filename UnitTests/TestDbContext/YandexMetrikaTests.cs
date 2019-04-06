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


    public class YandexMetrikaTests
    {
        private YandexMetrikaDbContext _metrikaDbContext { get; set; }

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
            var allMetrika = _metrikaDbContext.YandexMetrikaTable
                .Select(x => new SelectYandexMetrikaTestDto()
                {
                    title = x.Title,
                    eventTime = x.EventTime,
                    goodEvent = x.GoodEvent,
                    javaEnable = x.JavaEnable,
                    watchID = x.WatchID
                })
                .Take(10)
                .ToList();

            Assert.AreEqual(10, allMetrika.Count);
        }

        public class A : B
        {
        }

        public class B
        {
        }

        [Test]
        public void SelectWhereTakeTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var allMetrika = _metrikaDbContext.YandexMetrikaTable.Select(x => new SelectYandexMetrikaTestDto()
                {
                    title = x.Title,
                    eventTime = x.EventTime,
                    goodEvent = x.GoodEvent,
                    javaEnable = x.JavaEnable,
                    watchID = x.WatchID
                }).Where(x => x.title != "")
                .Take(10)
                .ToList();
            Console.WriteLine(sw.Elapsed);
            Assert.False(allMetrika.Any(x => x.title == ""));
        }

        [Test]
        public void SelectWhereSimpleSelectTakeTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            var allMetrika1 = _metrikaDbContext.YandexMetrikaTable.Select(x => new SelectYandexMetrikaTestDto()
                {
                    title = x.Title,
                    eventTime = x.EventTime,
                    goodEvent = x.GoodEvent,
                    javaEnable = x.JavaEnable,
                    watchID = x.WatchID
                }).Where(x => x.title != "")
                .Select(x => x.title)
                .Take(10)
                .ToList();
            Console.WriteLine(sw.Elapsed);
        }

        [Test]
        public void SelectWhereAnonymouseSelectTakeTest()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable.Select(x => new SelectYandexMetrikaTestDto()
                {
                    title = x.Title,
                    eventTime = x.EventTime,
                    goodEvent = x.GoodEvent,
                    javaEnable = x.JavaEnable,
                    watchID = x.WatchID
                }).Where(x => x.title != "" && x.javaEnable == 0)
                .Select(x => new {x.title, x.javaEnable})
                .Take(10)
                .ToList();

            Assert.True(allMetrika.Any(x => x.title != "" && x.javaEnable == 0));
        }


        [Test]
        public void SelectWhereConcreteSelectTakeTest()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable.Select(x => new SelectYandexMetrikaTestDto()
                {
                    title = x.Title,
                    eventTime = x.EventTime,
                    goodEvent = x.GoodEvent,
                    javaEnable = x.JavaEnable,
                    watchID = x.WatchID
                }).Where(x => x.title != "" && x.javaEnable == 0)
                .Select(x => new SelectYandexMetrikaTestTitleDto()
                {
                    t = x.title,
                    e = x.eventTime,
                    je = x.javaEnable
                })
                .Take(10)
                .ToList();

            Assert.True(allMetrika.Any(x => x.t != "" && x.je == 0));
        }

        [Test]
        public void WhereTakeTest()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable
                .Where(x => x.Title != "" && x.JavaEnable == 0)
                .Take(10)
                .ToList();
            Assert.True(allMetrika.Any(x => x.Title != "" && x.JavaEnable == 0));
        }

        [Test]
        public void OrderByTest()
        {
            var allMetrika = _metrikaDbContext.YandexMetrikaTable
                .Take(10)
                .OrderBy(x => new {x.Age, x.Sex}).ToList();

            var lastItem = allMetrika.Last();
            Assert.True(allMetrika.TrueForAll(x => x.Age <= lastItem.Age));
            Assert.True(allMetrika.TrueForAll(x => x.Sex <= lastItem.Sex));
        }

        [Test]
        public void ArraySelectTest()
        {
            var all = _metrikaDbContext.YandexMetrikaTable.Select(x => x.RefererRegions).Take(10).ToList();
        }
    }
}