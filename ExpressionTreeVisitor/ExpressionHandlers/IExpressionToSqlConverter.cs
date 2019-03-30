using System.Linq.Expressions;
using ExpressionTreeVisitor;

namespace DbContext
{
    public interface IExpressionToSqlConverter
    {
        string GetSql(ForSqlRequestInfo sqlRequestInfo);
    }
}