using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ExpressionToSqlConverter : IQuery<ForSqlRequestInfo,string>
    {
        private readonly ISqlRequestHandler _sqlRequestHandler;

        public ExpressionToSqlConverter(ISqlRequestHandler sqlRequestHandler)
        {
            _sqlRequestHandler = sqlRequestHandler;
        }

        //TODO добавить нормальный pattern mathing
        public string Query(ForSqlRequestInfo sqlRequestInfo)
        {
            var sql = _sqlRequestHandler.Handle(sqlRequestInfo);
            var sqlString = $"SELECT {sql.Select} FROM {sql.TableName} ";
            if (sql.Where.Any())
                sqlString += $" WHERE {sql.Where} ";
            if (sql.OrderBy != string.Empty)
                sqlString += $"ORDER BY {sql.OrderBy} ";
            if (sql.Take != string.Empty)
                sqlString += $"LIMIT {sql.Take}";

            return sqlString;
        }
    }
}