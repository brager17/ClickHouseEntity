using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class InsertInfo
    {
        public string TableName { get; set; }
        public string[] InsertColumns { get; set; }
        public object[][] Values { get; set; }
    }
}