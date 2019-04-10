using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class DataHandler : IDataHandler
    {
        private readonly SimpleTypeObjectBinder _valueTypeBinder;
        private readonly ComplexObjectBinder _complexTypeBinder;
        private readonly ArrayObjectBinder _simpleArrayBinder;

        public DataHandler(SimpleTypeObjectBinder valueTypeBinder, ComplexObjectBinder complexTypeBinder,
            ArrayObjectBinder simpleArrayBinder)
        {
            _valueTypeBinder = valueTypeBinder;
            _complexTypeBinder = complexTypeBinder;
            _simpleArrayBinder = simpleArrayBinder;
        }


        // todo закешировать MethodInfo
        public T Handle<T>(IDataReader reader)
        {
            var type = typeof(T).GenericTypeArguments.Single();
            if (type.IsSimpleType())
            {
                return (T) GetMethod<T>(typeof(SimpleTypeObjectBinder)).Invoke(_valueTypeBinder, new object[] {reader});
            }

            if (type.IsArray)
            {
                return (T) GetMethod<T>(typeof(ArrayObjectBinder))
                    .Invoke(_simpleArrayBinder, new object[] {reader});
            }

            if (type.IsClassType())
            {
                var methodInfo = GetMethod<T>(typeof(ComplexObjectBinder));
                return (T) methodInfo.Invoke(_complexTypeBinder, new object[] {reader});
            }


            throw new NotSupportedException();
        }

        private MethodInfo GetMethod<T>(Type handlerType) =>
            handlerType.GetMethod("Handle").MakeGenericMethod(typeof(T).GenericTypeArguments.Single());
    }
}