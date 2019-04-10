
namespace ClickHouseDbContextExntensions.CQRS
{
    public class Aggregate4Query<TIn, TOut1, TOut2, TOut3, TOut4> : BaseAggregateQuery<TIn, TOut4>
    {
        public Aggregate4Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1, IQuery<TOut2, TOut3> query2,
            IQuery<TOut3, TOut4> query3) : base(query0, query1, query2, query3)
        {
        }
    }
}