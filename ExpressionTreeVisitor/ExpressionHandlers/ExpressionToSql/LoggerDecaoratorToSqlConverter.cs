using System.Collections.Generic;
using ClickHouseDbContextExntensions.CQRS;
using Context;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class LoggerDecaoratorToSqlConverter<TIn>: IQuery<TIn, string>
    {
        private readonly IQuery<TIn, string> _expressionToSqlConverter;
        private readonly IEnumerable<IDbLogger> _loggers;

        public LoggerDecaoratorToSqlConverter(
            IQuery<TIn, string> expressionToSqlConverter,
            IEnumerable<IDbLogger> loggers)
        {
            _expressionToSqlConverter = expressionToSqlConverter;
            _loggers = loggers;
        }

        public string Query(TIn sqlRequestInfo)
        {
            var result = _expressionToSqlConverter.Query(sqlRequestInfo);
            foreach (var loggerToSqlConverter in _loggers)
                loggerToSqlConverter.WriteLog(LogLevel.Success, result);

            return result;
        }
    }
}