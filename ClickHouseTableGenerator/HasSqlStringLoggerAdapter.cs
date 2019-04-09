using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;

namespace ClickHouseTableGenerator
{
    public class HasSqlStringInfoLoggerAdapter : IQuery<HasSqlStringInfo, string>
    {
        public string Query(HasSqlStringInfo input)
        {
            return input.Sql;
        }
    }
}