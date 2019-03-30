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
        private readonly IValueTypeHandler _valueTypeHandler;
        private readonly IEntityTypeHandler _classTypeTypeHandler;

        public ModelBinder(IValueTypeHandler valueTypeHandler, IEntityTypeHandler classTypeTypeHandler)
        {
            _valueTypeHandler = valueTypeHandler;
            _classTypeTypeHandler = classTypeTypeHandler;
        }

        public T Bind<T>(IDataReader reader, BindInfo bindInfo)
        {
            var type = typeof(T).GenericTypeArguments.Single();
            if (type.IsSimpleType())
            {
                var invoke = typeof(ValueTypeHandler).GetMethod("Handle").MakeGenericMethod(type)
                    .Invoke(_valueTypeHandler, new[] {reader});

                return (T) invoke;
            }

            if (type.IsClass)
            {
                var invoke = typeof(ClassTypeHandler).GetMethod("Handle").MakeGenericMethod(type)
                    .Invoke(_classTypeTypeHandler, new object[] {reader, bindInfo});
                return (T) invoke;
            }


            throw new NotSupportedException();
        }
    }
}