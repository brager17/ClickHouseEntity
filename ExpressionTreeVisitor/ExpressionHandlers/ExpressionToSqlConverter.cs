using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ExpressionToSqlConverter : IExpressionToSqlConverter
    {
        private readonly ISqlRequestHandler _sqlRequestHandler;
        private AggregateLinqVisitor BaseVisitor { get; set; }

        public ExpressionToSqlConverter(ISqlRequestHandler sqlRequestHandler, AggregateLinqVisitor baseVisitor)
        {
            _sqlRequestHandler = sqlRequestHandler;
            BaseVisitor = baseVisitor;
        }

        //TODO добавить нормальный pattern mathing
        public string GetSql(ForSqlRequestInfo sqlRequestInfo)
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