namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IQuery<in TIn, out TOut>
    {
        TOut Query(TIn input);
    }
}