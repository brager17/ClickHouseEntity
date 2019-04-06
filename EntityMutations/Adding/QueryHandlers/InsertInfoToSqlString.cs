using System.Data;
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
            var valuesStr = "";
            for (int i = 0; i < input.Values.Length; i++)
            {
                valuesStr += $"({string.Join(",", input.Values[i])}),\n";
            }

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
}