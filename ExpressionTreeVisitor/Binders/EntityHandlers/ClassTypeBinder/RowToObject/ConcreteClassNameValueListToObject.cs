using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;
using ReflectionCache;

namespace DbContext
{
    public class ConcreteClassNameValueListToObject : IGenericQuery<PropertiesNameValues>
    {
        private readonly IQuery<TypePropertiesInfo, Delegate> _delegateBuilder;

        public ConcreteClassNameValueListToObject(IQuery<TypePropertiesInfo, Delegate> delegateBuilder)
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


    public class LambdaCompileQuery : IQuery<LambdaExpression, Delegate>
    {
        public Delegate Query(LambdaExpression input) => input.Compile();
    }

    public class BuildCreationInitializerLambda : IQuery<TypePropertiesInfo, LambdaExpression>
    {
        public LambdaExpression Query(TypePropertiesInfo input)
        {
            var props = input.Properties.ToArray();
            var stringType = typeof(string);
            var lambdaParameters = props.Select(x =>
                {
                    if (x.PropertyType.IsArray && x != stringType)
                        return Expression.Parameter(typeof(object[]));
                    return Expression.Parameter(x.PropertyType);
                })
                .ToArray();
            var ctor = Expression.New(input.Type);
            var arr = new MemberAssignment[props.Length];
            for (var i = 0; i < arr.Length; i++)
            {
                if (lambdaParameters[i].Type.IsArray && lambdaParameters[i].Type != stringType)
                {
                    //это хак из-за того, что Ado.net ClickHouseProvider возвращает массивы только типа object
                    var convertMethod =
                        ConvertArrayMethodInfo.MakeGenericMethod(props[i].PropertyType.GetElementType());
                    arr[i] = Expression.Bind(props[i], Expression.Call(null, convertMethod, lambdaParameters[i]));
                    continue;
                }

                arr[i] = Expression.Bind(props[i], lambdaParameters[i]);
            }

            var newInit = Expression.MemberInit(ctor, arr);
            var lambda = Expression.Lambda(newInit, lambdaParameters);
            return lambda;
        }

        // todo убрать как только ClickHouseAdoNet начнет возвращать типизированные массивы а не всегда object[]
        private static TItem[] ConvertArray<TItem>(object[] objects)
        {
            return objects.Select(x => Convert.ChangeType(x, typeof(TItem))).Cast<TItem>().ToArray();
        }

        private static MethodInfo ConvertArrayMethodInfo = typeof(BuildCreationInitializerLambda)
            .GetMethod("ConvertArray", BindingFlags.NonPublic | BindingFlags.Static);

        //
    }

    public class TypePropertiesInfo
    {
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((dynamic) obj).Type == Type;
        }

        public Type Type { get; set; }
        public IEnumerable<PropertyInfo> Properties { get; set; }
    }
}