using System.Collections.Generic;
using System.Linq;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class OrderOperationRequestHandler : IOperationRequestHandle<IEnumerable<OrderInfo>>
    {
        //todo сделано для OrderBy,OrderByDecsending
        public string Handle(IEnumerable<OrderInfo> operationInfo) =>
            string.Join(',',
                operationInfo.Select(x => $"{x.OrderString} {x.OrderType.GetNameAttributeValueEnumMember()}"));
    }
}