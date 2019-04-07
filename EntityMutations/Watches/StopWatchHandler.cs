using System.Collections.Generic;
using System.Diagnostics;
using ClickHouseDbContextExntensions.CQRS;
using Context;

namespace EntityTracking
{
    public class StopWatchHandler<TIn> : IHandler<TIn>
    {
        private readonly IEnumerable<IDbLogger> _loggers;
        private readonly IHandler<TIn> _handler;

        public StopWatchHandler(IHandler<TIn> handler, IEnumerable<IDbLogger> loggers)
        {
            _loggers = loggers;
            _handler = handler;
        }

        public void Handle(TIn input)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _handler.Handle(input);
            var time = stopWatch.Elapsed;
            stopWatch.Stop();
            foreach (var logger in _loggers)
            {
                logger.WriteLog(LogLevel.Success, time + " millisecond Handler " + _handler.GetType().Name);
            }
        }
    }
}