using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;

namespace DbContext
{
    public class AnonymousClassNameValueListToObject : IGenericQuery<PropertiesNameValues>
    {
        // todo кэшировать лямбду
        
        public T Query<T>(PropertiesNameValues cells)
        {
            var props = cells.Properties;
            // строим констуктор

            var constantExpressions = new UnaryExpression[cells.Properties.Length];
            for (int i = 0; i < cells.Properties.Length; i++)
            {
                constantExpressions[i] =
                    Expression.Convert(Expression.Constant(cells.Values[i]), cells.Properties[i].PropertyType);
            }
           
            var ctor = typeof(T).GetConstructors().Single();
            var expressionNew = Expression.New(ctor, constantExpressions);
            var ctorLambda = Expression.Lambda<Func<T>>(expressionNew);
            return ctorLambda.Compile().Invoke();
        }
    }

}