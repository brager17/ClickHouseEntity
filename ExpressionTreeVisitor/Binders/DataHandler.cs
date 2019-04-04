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
    public class DataHandler : IDataHandler
    {
        private readonly ObjectBinder _valueTypeBinder;
        private readonly ObjectBinder _complexTypeBinder;
        private readonly ObjectBinder _simpleArrayBinder;

        public DataHandler(ObjectBinder valueTypeBinder, ObjectBinder complexTypeBinder, ObjectBinder simpleArrayBinder)
        {
            _valueTypeBinder = valueTypeBinder;
            _complexTypeBinder = complexTypeBinder;
            _simpleArrayBinder = simpleArrayBinder;
        }


        // todo закешировать MethodInfo
        public T Handle<T>(IDataReader reader)
        {
            var type = typeof(T).GenericTypeArguments.Single();
            var makeGenericMethod = typeof(ObjectBinder).GetMethod("Handle").MakeGenericMethod(type);
            if (type.IsSimpleType())
            {
                var invoke = makeGenericMethod.Invoke(_valueTypeBinder, new[] {reader});
                return (T) invoke;
            }

            if (type.IsArray)
            {
                var invoke = makeGenericMethod.Invoke(_simpleArrayBinder, new[] {reader});
                return (T) invoke;
            }

            if (type.IsClassType())
            {
                var invoke = makeGenericMethod.Invoke(_complexTypeBinder, new object[] {reader});
                return (T) invoke;
            }


            throw new NotSupportedException();
        }
    }

  
}