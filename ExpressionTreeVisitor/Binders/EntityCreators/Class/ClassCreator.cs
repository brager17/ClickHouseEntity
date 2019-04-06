using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;
using ReflectionCache;

namespace DbContext
{
    public class ClassCreator : IGenericQuery<PropertiesNameValues>
    {
        private readonly IQuery<TypePropertiesInfo, Delegate> _delegateBuilder;

        public ClassCreator(IQuery<TypePropertiesInfo, Delegate> delegateBuilder)
        {
            _delegateBuilder = delegateBuilder;
        }

        public T Query<T>(PropertiesNameValues cells)
        {
            var func = _delegateBuilder.Query(new TypePropertiesInfo {Type = typeof(T), Properties = cells.Properties});
            var entity = (T) func.DynamicInvoke(cells.Values);
            return entity;
        }
    }


    [Info("Optimize code")]
    public class TypePropertiesInfo
    {
        // нужен для того, чтобы оптимизировать Cache
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        // нужен для того, чтобы оптимизировать Cache
        public override bool Equals(object obj)
        {
            return ((dynamic) obj).Type == Type;
        }

        public Type Type { get; set; }
        public IEnumerable<PropertyInfo> Properties { get; set; }
    }
}