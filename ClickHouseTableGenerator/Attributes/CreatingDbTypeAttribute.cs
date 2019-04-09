using ExpressionTreeVisitor;

namespace ClickHouseTableGenerator
{
    public class CreatingDbTypeAttribute : NameAttribute
    {
        public CreatingDbTypeAttribute(string name) : base(name)
        {
        }
    }
}