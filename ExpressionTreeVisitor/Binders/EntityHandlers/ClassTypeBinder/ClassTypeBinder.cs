using System.Collections.Generic;
using ExpressionTreeVisitor;

namespace DbContext
{
    //todo добавить кэширование MethodInfo
    public class ClassTypeBinder : IComplexEntityBinder
    {
        private readonly IRowToObject _concreteClassRowToObject;
        private readonly IRowToObject _anonymousClassRowToObject;

        public ClassTypeBinder(IRowToObject concreteClassRowToObject, IRowToObject anonymousClassRowToObject)
        {
            _concreteClassRowToObject = concreteClassRowToObject;
            _anonymousClassRowToObject = anonymousClassRowToObject;
        }

        public T Handle<T>(IEnumerable<Cell> cells)
        {
            IRowToObject rowToObjectType;

            if (typeof(T).IsAnonymouseClass())
                rowToObjectType = _anonymousClassRowToObject;
            else
                rowToObjectType = _concreteClassRowToObject;
            var result = (T) rowToObjectType.GetType().GetMethod("Build").MakeGenericMethod(typeof(T))
                .Invoke(rowToObjectType, new object[] {cells});

            return result;
        }
    }
}