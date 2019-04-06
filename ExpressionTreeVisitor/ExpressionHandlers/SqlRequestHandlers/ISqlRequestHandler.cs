using ExpressionTreeVisitor;

namespace DbContext
{
    public interface ISqlRequestHandler
    {
        SqlRequest Handle(ForSqlRequestInfo linqInfo);
    }
}