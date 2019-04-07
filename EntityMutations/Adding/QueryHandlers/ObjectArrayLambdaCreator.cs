using System;
using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class ObjectArrayLambdaCreator<T> : IQuery<TypeInfo, Expression<Func<T, object[]>>>
    {
        public Expression<Func<T, object[]>> Query(TypeInfo input)
        {
            var properties = input.Type.GetProperties().ToArray();
            var parameter = Expression.Parameter(input.Type);
            var unaryExpressions = new UnaryExpression[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                unaryExpressions[i] = Expression.Convert(Expression.Property(parameter, properties[i]), typeof(object));
            var array = Expression.NewArrayInit(typeof(object), unaryExpressions);
            var lambda = Expression.Lambda<Func<T, object[]>>(array, parameter);
            return lambda;
        }
    }
}