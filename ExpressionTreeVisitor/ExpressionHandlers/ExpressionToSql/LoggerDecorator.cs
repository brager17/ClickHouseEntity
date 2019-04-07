using System.Collections.Generic;
using ClickHouseDbContextExntensions.CQRS;
using Context;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class LoggerDecorator<TIn> : IQuery<TIn, string>
    {
        private readonly IQuery<TIn, string> _query;
        private readonly IEnumerable<IDbLogger> _loggers;

        public LoggerDecorator(
            IQuery<TIn, string> query,
            IEnumerable<IDbLogger> loggers)
        {
            _query = query;
            _loggers = loggers;
        }

        public string Query(TIn input)
        {
            var result = _query.Query(input);
            foreach (var loggerToSqlConverter in _loggers)
                loggerToSqlConverter.WriteLog(LogLevel.Success, $"\n{result}\n");

            return result;
        }
    }


    public class LoggerDecoratorWithConverter<TIn, TOut> : IQuery<TIn, TOut>
    {
        private readonly IQuery<TIn, TOut> _query;
        private readonly IQuery<TOut, string> _loggerConverter;
        private readonly IEnumerable<IDbLogger> _loggers;

        public LoggerDecoratorWithConverter(
            IQuery<TIn, TOut> query,
            IQuery<TOut, string> loggerConverter,
            IEnumerable<IDbLogger> loggers)
        {
            _query = query;
            _loggerConverter = loggerConverter;
            _loggers = loggers;
        }

        public TOut Query(TIn input)
        {
            var result = _query.Query(input);
            foreach (var logger in _loggers) logger.WriteLog(LogLevel.Success, $"\n{_loggerConverter.Query(result)}\n");
            return result;
        }
    }
}