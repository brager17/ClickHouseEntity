using System.Data;
using System.Diagnostics;
using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class InsertInfoToSqlString : IQuery<InsertInfo, AddingSql>
    {
        public AddingSql Query(InsertInfo input)
        {
            var result = new AddingSql
            {
                Sql = $"INSERT INTO {input.TableName} ({string.Join(',', input.InsertColumns)}) VALUES @bulk",
                ClickHouseParameter = new ClickHouseParameter
                {
                    DbType = DbType.Object,
                    ParameterName = "bulk",
                    Value = input.Values
                }
            };
            return result;
        }
    }

    public class AddingSqlLoggingConverter : IQuery<AddingSql, string>
    {
        public string Query(AddingSql input)
        {
            var values = input.ClickHouseParameter.AsSubstitute();
            return input.Sql.Replace("@bulk", values);
        }
    }
}