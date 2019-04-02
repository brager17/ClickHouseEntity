using System.Linq;
using ClickDbContextInfrastructure;
using Context;
using ExpressionTreeVisitor;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests
{
    [TestFixture]
    public class BaseExpressionInfoVisitorTests
    {
        private TestDbContext context { get; set; }

        [SetUp]
        public void SetUp()
        {
            context = new TestDbContext();
        }

        [Test]
        public void Test1()
        {
            var s = new InfoVisitor();
            var expression = context.TestTables.Select(x => new Dto
                {
                    SomeStr = x.SomeString,
                    SomeInt1 = x.SomeInt
                })
                .Where(x => x.SomeInt1 != 12)
                .Select(x => new
                {
                    x.SomeStr, x.SomeInt1
                }).Select(x => x.SomeInt1);
            var ss = s.Visit(expression.Expression);
        }

      
    }
}