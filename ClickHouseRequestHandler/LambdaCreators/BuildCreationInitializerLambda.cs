using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    [Info("Optimize code")]
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
        // UsedImplicitly//
        private static TItem[] ConvertArray<TItem>(object[] objects)
        {
            return objects.Select(x => Convert.ChangeType(x, typeof(TItem))).Cast<TItem>().ToArray();
        }

        private static MethodInfo ConvertArrayMethodInfo = typeof(BuildCreationInitializerLambda)
            .GetMethod("ConvertArray", BindingFlags.NonPublic | BindingFlags.Static);

        //
    }
}