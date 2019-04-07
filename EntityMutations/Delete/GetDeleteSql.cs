using System;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;

namespace EntityTracking
{
    public class GetDeleteSql : IQuery<WhereSqlTableInfo, DeleteStr>
    {
        private readonly IOperationRequestHandle<Type> _tableNameRequestHandler;

        public GetDeleteSql(IOperationRequestHandle<Type> tableNameRequestHandler)
        {
            _tableNameRequestHandler = tableNameRequestHandler;
        }

        public DeleteStr Query(WhereSqlTableInfo input)
        {
            var tableName = _tableNameRequestHandler.Handle(input.TableType);
            var filter = input.WhereStr.SqlInfo;
            return new DeleteStr {Sql = $"ALTER TABLE {tableName} DELETE WHERE {filter}"};
        }
    }
}