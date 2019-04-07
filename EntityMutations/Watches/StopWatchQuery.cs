using System.Collections.Generic;
using System.Diagnostics;
using ClickHouseDbContextExntensions.CQRS;
using Context;

namespace EntityTracking
{
    public class StopWatchQuery<TIn, TOut> : IQuery<TIn, TOut>
    {
        private readonly IEnumerable<IDbLogger> _loggers;
        private readonly IQuery<TIn, TOut> _query;

        public StopWatchQuery(IQuery<TIn, TOut> query,IEnumerable<IDbLogger> loggers)
        {
            _loggers = loggers;
            _query = query;
        }

        public TOut Query(TIn input)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = _query.Query(input);
            var time = stopWatch.Elapsed;
            stopWatch.Stop();
            foreach (var logger in _loggers)
            {
                logger.WriteLog(LogLevel.Success, time + " millisecond Query " + _query.GetType().Name);

            }

            return result;
        }
    }
}