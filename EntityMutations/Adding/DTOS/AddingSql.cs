using ClickHouse.Ado;
using ClickHouseDbContextExntensions.DTOS;

namespace EntityTracking
{
    public class AddingSql : HasSqlStringInfo
    {
        public ClickHouseParameter ClickHouseParameter { get; set; }
    }
}