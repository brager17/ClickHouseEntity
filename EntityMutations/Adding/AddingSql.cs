using ClickHouse.Ado;

namespace EntityTracking
{
    public class AddingSql
    {
        public string Sql { get; set; }
        public ClickHouseParameter ClickHouseParameter { get; set; }
    }
}