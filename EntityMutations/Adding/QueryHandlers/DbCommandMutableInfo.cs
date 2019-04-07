using ClickHouse.Ado;

namespace EntityTracking
{
    public class DbCommandMutableInfo<T>
    {
        public ClickHouseCommand Command { get; set; }
        public T Info { get; set; }
    }
}