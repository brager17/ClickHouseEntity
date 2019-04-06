using System.Collections.Generic;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    //todo добавить кэширование MethodInfo
    public class ClassTypeBinder : IClassBinder
    {
        private readonly ITInGenericQuery<PropertiesNameValues> _concreteClassNameValueListToObject;
        private readonly ITInGenericQuery<PropertiesNameValues> _anonymousClassNameValueListToObject;


        public ClassTypeBinder(
            ITInGenericQuery<PropertiesNameValues> concreteClassNameValueListToObject,
            ITInGenericQuery<PropertiesNameValues> anonymousClassNameValueListToObject)
        {
            _concreteClassNameValueListToObject = concreteClassNameValueListToObject;
            _anonymousClassNameValueListToObject = anonymousClassNameValueListToObject;
        }

        public T Handle<T>(PropertiesNameValues cells)
        {
            ITInGenericQuery<PropertiesNameValues> nameValueListToObjectType;

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