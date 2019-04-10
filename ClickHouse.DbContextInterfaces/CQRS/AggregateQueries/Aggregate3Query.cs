namespace ClickHouseDbContextExntensions.CQRS
{
    public class Aggregate3Query<TIn, TOut1, TOut2, TOut3> : BaseAggregateQuery<TIn, TOut3>
    {
        public Aggregate3Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1, IQuery<TOut2, TOut3> query2)
            : base(query0, query1, query2)
        {
        }
    }
}