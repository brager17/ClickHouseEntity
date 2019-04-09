namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IMutableQuery<in TIn, out TOut> : IQuery<TIn, TOut>
    {
    }
}