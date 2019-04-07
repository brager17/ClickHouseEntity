using ClickHouse.Ado;

namespace EntityTracking
{
    public class AddingSql : HasSqlStringInfo
    {
        public ClickHouseParameter ClickHouseParameter { get; set; }
    }
}