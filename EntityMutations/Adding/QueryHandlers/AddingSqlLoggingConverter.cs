using ClickHouseDbContextExntensions.CQRS;

namespace EntityTracking
{
    public class AddingSqlLoggingConverter : IQuery<AddingSql, string>
    {
        public string Query(AddingSql input)
        {
            var values = input.ClickHouseParameter.AsSubstitute();
            return input.Sql.Replace("@bulk", values);
        }
    }
}