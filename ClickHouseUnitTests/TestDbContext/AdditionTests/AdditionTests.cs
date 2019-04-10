using System.Diagnostics;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnitTests.TestDbContext;

namespace Tests.AdditionTests
{
    [TestFixture]
    public class AdditionTests : BaseTestTableOperations
    {
        [Test]
        public void AddTest1000()
        {
            var data = AddData.Data1000;
            _context.TestTables.Add(AddData.Data1000);
            AssertCount(1000);
        }

        [Test]
        public void AddTest10000()
        {
            _context.TestTables.Add(AddData.Data10000);
            AssertCount(10000);
        }

        [Test]
        public void AddTest100000()
        {
            _context.TestTables.Add(AddData.Data100000);

            AssertCount(100000);
        }


//        [Test]
        public void AddTest1000000()
        {
            _context.TestTables.Add(AddData.Data1000000);
            AssertCount(1000000);
        }
    }
}