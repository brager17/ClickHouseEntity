using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    [Info("Optimize code")]
    public class BuildCreatorCtorLambda : IQuery<TypePropertiesInfo, LambdaExpression>
    {
        public LambdaExpression Query(TypePropertiesInfo input)
        {
            var ctor = input.Type.GetConstructors().Single();
            var props = input.Properties.ToArray();
            var propertiesCount = props.Length;
            var expressionCtorArguments = new ParameterExpression[propertiesCount];
            for (int i = 0; i < propertiesCount; i++)
                expressionCtorArguments[i] = Expression.Parameter(props[i].PropertyType);

            var ctorExpression = Expression.New(ctor, expressionCtorArguments);
            var lambda = Expression.Lambda(ctorExpression, expressionCtorArguments);
            return lambda;
        }
    }
}