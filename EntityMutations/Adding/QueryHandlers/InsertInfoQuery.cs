using System.Linq;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class InsertInfoQuery<T> : IQuery<T[], InsertInfo>
    {
        public InsertInfo Query(T[] input)
        {
            var properties = typeof(T).GetProperties().ToArray();
            var tableName = typeof(T).GetCustomAttribute<TableNameAttribute>().Name;
            var insertPropertiesName = new string[properties.Length];
            var countRows = input.Length;
            var countProps = properties.Length;
            var values = new object[countRows][];

            for (int i = 0; i < countProps; i++)
            {
                insertPropertiesName[i] = properties[i].GetColumnName();
            }

            for (int i = 0; i < countRows; i++)
            {
                values[i] = new object[countProps];
                for (int j = 0; j < countProps; j++)
                {
                    values[i][j] = properties[j].GetValue(input[i]);
                }
            }

            return new InsertInfo
            {
                Values = values, TableName = tableName, InsertColumns = insertPropertiesName
            };
        }
    }
}