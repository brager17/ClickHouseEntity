using System;
using System.Linq;
using System.Reflection;

namespace DbContext
{
    public class TableNameRequestHandler : IOperationRequestHandle<Type>
    {
        public string Handle(Type operationInfo)
        {
            var result = operationInfo.GetCustomAttributes<TableNameAttribute>()?.Single()?.Name ?? operationInfo.Name;
            return $" {result} ";
        }
    }
}