using ClickHouseDbContextExntensions.CQRS;

namespace EntityTracking.Delete
{
    public class DeleteStrLoggingConverter : IQuery<DeleteStr, string>
    {
        public string Query(DeleteStr input)
        {
            return input.Sql;
        }
    }
}