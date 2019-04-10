using System.Collections.Generic;

namespace ClickHouseDbContextExntensions.CQRS
{
    public class CacheQueryWithHashCodeDto<TIn, TOut> : IQuery<TIn, TOut>
    {
        private readonly Dictionary<TIn, TOut> _dic;
        private readonly IQuery<TIn, TOut> _query;

        public CacheQueryWithHashCodeDto(IQuery<TIn, TOut> query)
        {
            _query = query;
            _dic = new Dictionary<TIn, TOut>();
        }

        public TOut Query(TIn input)
        {
            TOut value;
            if (_dic.TryGetValue(input, out value))
                return value;
            value = _query.Query(input);
            _dic.Add(input, value);
            return value;
        }
    }
}