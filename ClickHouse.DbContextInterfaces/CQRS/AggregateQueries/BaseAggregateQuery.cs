using System.Linq;

namespace ClickHouseDbContextExntensions.CQRS
{
    public abstract class BaseAggregateQuery<TIn, TOut> : IQuery<TIn, TOut>
    {
        protected object[] queries { get; set; }

        protected BaseAggregateQuery(params object[] queries)
        {
            this.queries = queries;
        }

        public TOut Query(TIn input) =>
            queries.Aggregate<object, dynamic>(input, (current, query) => ((dynamic) query).Query(current));
    }
}