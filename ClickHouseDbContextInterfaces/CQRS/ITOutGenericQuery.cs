namespace ClickHouseDbContextExntensions.CQRS
{
    public interface ITOutGenericQuery<out TOut>
    {
        TOut Query<TIn>(TIn input);
    }
}