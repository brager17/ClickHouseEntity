using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ExpressionToSqlConverter : IExpressionToSqlConverter
    {
        private readonly ISqlRequestHandler _sqlRequestHandler;
        private BaseExpressionVisitor BaseVisitor { get; set; }

        public ExpressionToSqlConverter(ISqlRequestHandler sqlRequestHandler, BaseExpressionVisitor baseVisitor)
        {
            _sqlRequestHandler = sqlRequestHandler;
            BaseVisitor = baseVisitor;
        }

        public string GetSql(ForSqlRequestInfo sqlRequestInfo)
        {
            var sql = _sqlRequestHandler.Handle(sqlRequestInfo);
            var sqlString = $"SELECT {sql.Select} FROM {sql.TableName} ";
            if (sql.Where.Any())
                sqlString += $" WHERE {sql.Where} ";
            return sqlString;
        }

        public string SelectParser(SelectInfo info)
        {
            //SELECT some_int FROM MYTABLE
            return string.Empty;
        }
    }
}