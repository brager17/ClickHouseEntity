using System;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    public class GetDeleteSql : IQuery<WhereSqlTableInfo, DeleteStr>
    {

        public DeleteStr Query(WhereSqlTableInfo input)
        {
            var tableName = input.TableType.GetClassAttributeKey<string>();
            var filter = input.WhereStr.SqlInfo;
            return new DeleteStr {Sql = $"ALTER TABLE {tableName} DELETE WHERE {filter}"};
        }
    }
}