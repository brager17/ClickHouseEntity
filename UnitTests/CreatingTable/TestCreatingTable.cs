using System;
using ClickHouseTableGenerator;
using DbContext;
using ExpressionTreeVisitor;
using NUnit.Framework;

namespace UnitTests.CreatingTable
{
    [IndexGranularity(9000)]
    [TableName("TestCreatingTable")]
    public class TestCreatingTable
    {
        [PartitionKey] public DateTime Event { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [OrderKey] public int Age { get; set; }
    }
}