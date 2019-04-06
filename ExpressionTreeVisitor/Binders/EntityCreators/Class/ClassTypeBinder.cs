using System.Collections.Generic;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    //todo добавить кэширование MethodInfo
    public class ClassTypeBinder : IClassBinder
    {
        private readonly IGenericQuery<PropertiesNameValues> _concreteClassNameValueListToObject;
        private readonly IGenericQuery<PropertiesNameValues> _anonymousClassNameValueListToObject;


        public ClassTypeBinder(
            IGenericQuery<PropertiesNameValues> concreteClassNameValueListToObject,
            IGenericQuery<PropertiesNameValues> anonymousClassNameValueListToObject)
        {
            _concreteClassNameValueListToObject = concreteClassNameValueListToObject;
            _anonymousClassNameValueListToObject = anonymousClassNameValueListToObject;
        }

        public T Handle<T>(PropertiesNameValues cells)
        {
            IGenericQuery<PropertiesNameValues> nameValueListToObjectType;

            if (typeof(T).IsAnonymouseClass())
                nameValueListToObjectType = _anonymousClassNameValueListToObject;
            else
                nameValueListToObjectType = _concreteClassNameValueListToObject;
            var result = (T) _concreteClassNameValueListToObject.GetType().GetMethod("Query").MakeGenericMethod(typeof(T))
                .Invoke(nameValueListToObjectType, new object[] {cells});

            return result;
        }
    }
}