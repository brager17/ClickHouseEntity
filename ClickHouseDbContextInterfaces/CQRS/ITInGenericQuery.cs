namespace ClickHouseDbContextExntensions.CQRS
{
    public interface ITInGenericQuery<in TIn>
    {
        TOut Query<TOut>(TIn input);
    }
}