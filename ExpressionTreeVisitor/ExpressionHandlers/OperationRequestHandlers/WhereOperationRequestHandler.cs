using System.Collections.Generic;
using System.Linq;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class WhereOperationRequestHandler : IOperationRequestHandle<IEnumerable<WhereInfo>>
    {
        public string Handle(IEnumerable<WhereInfo> operationInfo) =>
            string.Join(" ", operationInfo.Select(x => x._WhereInfo.SqlInfo));
    }
}