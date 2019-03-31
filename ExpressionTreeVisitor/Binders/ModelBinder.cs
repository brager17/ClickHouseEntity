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
        private readonly IEntityTypeBinder _valueTypeBinder;
        private readonly IEntityTypeBinder _classTypeTypeBinder;

        public ModelBinder(IEntityTypeBinder valueTypeBinder, IEntityTypeBinder classTypeTypeBinder)
        {
            _valueTypeBinder = valueTypeBinder;
            _classTypeTypeBinder = classTypeTypeBinder;
        }

        
        // todo закешировать MethodInfo
        public T Bind<T>(IDataReader reader)
        {
            var type = typeof(T).GenericTypeArguments.Single();
            if (type.IsSimpleType())
            {
                var invoke = typeof(ValueTypeBinder).GetMethod("Handle").MakeGenericMethod(type)
                    .Invoke(_valueTypeBinder, new[] {reader});

                return (T) invoke;
            }

            if (type.IsClass)
            {
                var invoke = typeof(ClassTypeBinder).GetMethod("Handle").MakeGenericMethod(type)
                    .Invoke(_classTypeTypeBinder, new object[] {reader});
                return (T) invoke;
            }


            throw new NotSupportedException();
        }
    }
}