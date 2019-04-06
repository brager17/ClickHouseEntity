using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionCache;

namespace ClickHouseDbContextExntensions.CQRS
{
    public class Condition<TIn, TOut>
    {
        public Func<TIn, bool> _predicate { get; set; }
        public IQuery<TIn, TOut> _selector { get; set; }
    }

    public class ConditionQuery<TIn, TOut> : IQuery<TIn, TOut>
    {
        private readonly IEnumerable<Condition<TIn, TOut>> _list;

        public ConditionQuery(IEnumerable<Condition<TIn, TOut>> list)
        {
            _list = list;
        }

        public TOut Query(TIn input) =>
            (_list.FirstOrDefault(xx => xx._predicate(input))
             ?? throw new ArgumentException("Не удалось найти подходящее значение"))
            ._selector.Query(input);
    }
}