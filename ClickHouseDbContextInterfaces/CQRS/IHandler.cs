using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }

    public interface IQuery<in TIn, out TOut>
    {
        TOut Query(TIn input);
    }

    public interface ITOutGenericQuery<out TOut>
    {
        TOut Query<TIn>(TIn input);
    }

    public interface ITInGenericQuery<in TIn>
    {
        TOut Query<TOut>(TIn input);
    }


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

    #region aggregateQueries

    public class Aggregate2Query<TIn, TOut1, TOut2> : BaseAggregateQuery<TIn, TOut2>
    {
        public Aggregate2Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1)
            : base(query0, query1)
        {
        }
    }

    public class Aggregate3Query<TIn, TOut1, TOut2, TOut3> : BaseAggregateQuery<TIn, TOut3>
    {
        public Aggregate3Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1, IQuery<TOut2, TOut3> query2)
            : base(query0, query1, query2)
        {
        }
    }

    public class Aggregate4Query<TIn, TOut1, TOut2, TOut3, TOut4> : BaseAggregateQuery<TIn, TOut4>
    {
        public Aggregate4Query(IQuery<TIn, TOut1> query0, IQuery<TOut1, TOut2> query1, IQuery<TOut2, TOut3> query2,
            IQuery<TOut3, TOut4> query3) : base(query0, query1, query2, query3)
        {
        }
    }

    #endregion

    /// <summary>
    /// Нужно писать максимально производительный 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEquatableByFunc<T>
    {
        bool Equals(T item1, T item2);
    }

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
                {
                    query = arr[i].Value;
                    break;
                }

                i++;
            }

            if (query != null)
                return query;
            query = _query.Query(input);
            _dic.TryAdd(input, query);
            return query;
        }
    }

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