using System.Collections.Generic;
using ExpressionTreeVisitor;

namespace DbContext
{
    //todo добавить кэширование MethodInfo
    public class ClassTypeBinder : IComplexEntityBinder
    {
        private readonly INameValueListToObject _concreteClassNameValueListToObject;
        private readonly INameValueListToObject _anonymousClassNameValueListToObject;

        public ClassTypeBinder(INameValueListToObject concreteClassNameValueListToObject, INameValueListToObject anonymousClassNameValueListToObject)
        {
            _concreteClassNameValueListToObject = concreteClassNameValueListToObject;
            _anonymousClassNameValueListToObject = anonymousClassNameValueListToObject;
        }

        public T Handle<T>(IEnumerable<NameValue> cells)
        {
            INameValueListToObject nameValueListToObjectType;

            if (typeof(T).IsAnonymouseClass())
                nameValueListToObjectType = _anonymousClassNameValueListToObject;
            else
                nameValueListToObjectType = _concreteClassNameValueListToObject;
            var result = (T) nameValueListToObjectType.GetType().GetMethod("Build").MakeGenericMethod(typeof(T))
                .Invoke(nameValueListToObjectType, new object[] {cells});

            return result;
        }
    }
}