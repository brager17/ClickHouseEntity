using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ModelBinder : IModelBinder
    {
        private readonly ObjectBinder _valueTypeBinder;
        private readonly ObjectBinder _complexTypeBinder;

        public ModelBinder(ObjectBinder valueTypeBinder, ObjectBinder complexTypeBinder)
        {
            _valueTypeBinder = valueTypeBinder;
            _complexTypeBinder = complexTypeBinder;
        }


        // todo закешировать MethodInfo
        public T Bind<T>(IDataReader reader)
        {
            var type = typeof(T).GenericTypeArguments.Single();
            var makeGenericMethod = typeof(ObjectBinder).GetMethod("Handle").MakeGenericMethod(type);
            if (type.IsSimpleType())
            {
                var invoke = makeGenericMethod.Invoke(_valueTypeBinder, new[] {reader});

                return (T) invoke;
            }

            if (!type.IsSimpleType())
            {
                var invoke = makeGenericMethod.Invoke(_complexTypeBinder, new object[] {reader});
                return (T) invoke;
            }


            throw new NotSupportedException();
        }
    }
}