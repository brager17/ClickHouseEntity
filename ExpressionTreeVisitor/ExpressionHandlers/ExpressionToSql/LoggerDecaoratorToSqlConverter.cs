using System.Collections.Generic;
using Context;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class LoggerDecaoratorToSqlConverter : IExpressionToSqlConverter
    {
        private readonly IExpressionToSqlConverter _expressionToSqlConverter;
        private readonly IEnumerable<IDbLogger> _loggers;

        public LoggerDecaoratorToSqlConverter(IExpressionToSqlConverter expressionToSqlConverter,
            IEnumerable<IDbLogger> loggers)
        {
            _expressionToSqlConverter = expressionToSqlConverter;
            _loggers = loggers;
        }

        public string GetSql(ForSqlRequestInfo sqlRequestInfo)
        {
            var result = _expressionToSqlConverter.GetSql(sqlRequestInfo);
            foreach (var loggerToSqlConverter in _loggers)
                loggerToSqlConverter.WriteLog(LogLevel.Success, result);

            return result;
        }
    }
}