namespace ClickHouseDbContextExntensions.CQRS
{
    public class Aggregate2Query<TIn, TOut1, TOut2> : BaseAggregateQuery<TIn, TOut2>
    {
        public Aggregate2Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1)
            : base(query0, query1)
        {
        }
    }
}