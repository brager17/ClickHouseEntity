using ExpressionTreeVisitor;

namespace ClickHouseTableGenerator
{
    public class DbTypeAttribute : NameAttribute
    {
        public DbTypeAttribute(string name) : base(name)
        {
        }
    }
}