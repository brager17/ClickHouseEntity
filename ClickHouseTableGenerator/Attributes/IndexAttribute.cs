using ExpressionTreeVisitor;

namespace ClickHouseTableGenerator
{
    public class IndexAttribute : NameAttribute
    {
        public IndexAttribute(string name) : base(name)
        {
        }
    }
}