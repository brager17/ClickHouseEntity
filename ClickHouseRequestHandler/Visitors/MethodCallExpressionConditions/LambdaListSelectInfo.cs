using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class LambdaListSelectInfo
    {
        public MethodCallExpression MethodCallExpression { get; set; }
        public IEnumerable<SelectInfo> SelectInfos { get; set; }
        public AggregateLinqInfo AggregateLinqInfo { get; set; }
    }
}