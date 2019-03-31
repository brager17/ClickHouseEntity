using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DbContext
{
    public class ConcreteClassRowToObject : IRowToObject
    {
        // todo кэшировать лямбду
        public T Build<T>(IEnumerable<Cell> cells)
        {
            var props = typeof(T).GetProperties();

            // строим инициализатор
            var cellProps = props.Join(cells, x => x.Name, x => x.Alias,
                (x, y) => new {propertyType = x, value = y.Value});
            var assignments = cellProps.Select(x
                => Expression.Bind(x.propertyType,
                    Expression.Convert(Expression.Constant(x.value), x.propertyType.PropertyType)));

            var newExpression = Expression.New(typeof(T));
            var expressionCtor = Expression.MemberInit(newExpression, assignments);

            var lambda = Expression.Lambda<Func<T>>(expressionCtor);
            var func = lambda.Compile();
            var entity = func.Invoke();
            return entity;
        }
    }
}