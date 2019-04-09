namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IQueryFactory<in TIn, out TOut>
    {
        TOut Create(TIn input);
    }
}