using System.Collections.Generic;
using System.Linq;
using ExpressionTreeVisitor;

namespace DbContext
{
    public interface IOperationRequestHandle<T>
    {
        string Handle(T operationInfo);
    }

    public class OrderOperationRequestHandle : IOperationRequestHandle<IEnumerable<OrderInfo>>
    {
        //todo сделано для OrderBy,OrderByDecsending
        public string Handle(IEnumerable<OrderInfo> operationInfo) =>
            string.Join(',', operationInfo.Select(x => $"{x.OrderString} {x.OrderType.GetName()}"));
    }
}