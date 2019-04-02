using ExpressionTreeVisitor;

namespace DbContext
{
    public class TakeOperationRequestHandle : IOperationRequestHandle<TakeInfo>
    {
        public string Handle(TakeInfo operationInfo) =>
            operationInfo.Take.HasValue ? $"LIMIT {operationInfo.Take}" : null;
    }
}