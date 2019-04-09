using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;

namespace EntityTracking
{
    public class StubMutableQuery : IMutableQuery<DbCommandMutableInfo<HasSqlStringInfo>, ClickHouseCommand>
    {
        public ClickHouseCommand Query(DbCommandMutableInfo<HasSqlStringInfo> input) => input.Command;
    }
}