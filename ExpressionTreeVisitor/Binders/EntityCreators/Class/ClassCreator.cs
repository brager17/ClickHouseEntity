using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;
using ReflectionCache;

namespace DbContext
{
    public class ClassCreator : ITInGenericQuery<PropertiesNameValues>
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
            var tpi = (TypePropertiesInfo) obj;
            if (Type.Name != tpi.Type.Name)
                return false;
            if (Properties.Count() != tpi.Properties.Count())
                return false;
            var props = Properties.ToArray();
            var tpiProps = tpi.Properties.ToArray();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].GetHashCode() != tpiProps[i].GetHashCode())
                    return false;
            }

            return true;
        }

        public Type Type { get; set; }
        public IEnumerable<PropertyInfo> Properties { get; set; }
    }
}