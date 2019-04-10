using System.Collections.Generic;
using System.Linq;

namespace ClickHouseDbContextExntensions.CQRS
{
    public class CacheQuery<TIn, TOut> : IQuery<TIn, TOut>
    {
        private readonly IQuery<TIn, TOut> _query;
        private readonly IEquatableByFunc<TIn> _equatableByFunc;
        private readonly Dictionary<TIn, TOut> _dic;

        public CacheQuery(IQuery<TIn, TOut> query, IEquatableByFunc<TIn> equatableByFunc)
        {
            _query = query;
            _equatableByFunc = equatableByFunc;
            _dic = new Dictionary<TIn, TOut>();
        }

        public TOut Query(TIn input)
        {
            var arr = _dic.ToArray();
            var flag = true;
            var i = 0;
            var query = default(TOut);
            while (i < arr.Length)
            {
                var cond = _equatableByFunc.Equals(input, arr[i].Key);
                if (cond)
                    query = arr[i].Value;
                break;
                i++;
            }

            if (query != null)
                return query;
            query = _query.Query(input);
            _dic.TryAdd(input, query);
            return query;
        }
    }
}