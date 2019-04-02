using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class LinqInfo
    {
        public Expression Expression { get; set; }
        public LinqMethod LinqMethod { get; set; }
    }
}