using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DbContext
{
    public class ConcreteClassNameValueListToObject : INameValueListToObject
    {
        // todo кэшировать лямбду
        public T Build<T>(IEnumerable<NameValue> cells)
        {
            var props = typeof(T).GetProperties();

            // строим инициализатор
            var cellProps = props.Join(cells, x => x.Name, x => x.Name,
                (x, y) => new {propertyType = x, value = y.Value});
            var assignments = cellProps.Select(x
                =>
            {
                // todo обработать ошибку еще не заходя в этот хэндлер(ошибка конвертации)
                //todo обработать заполнение массива (ошибка в получении данных от Ado.Net Provider'a)
                try
                {
                    return Expression.Bind(x.propertyType,
                        Expression.Convert(Expression.Constant(x.value), x.propertyType.PropertyType));
                }
                catch (Exception e)
                {
                    var error =
                        $"The Value of the column {x.propertyType.Name} cannot be converted {x.propertyType.PropertyType} ";
                    Console.WriteLine(e);
                    throw new ArgumentException(error);
                }
            }).ToList();

            var newExpression = Expression.New(typeof(T));
            var expressionCtor = Expression.MemberInit(newExpression, assignments);

            var lambda = Expression.Lambda<Func<T>>(expressionCtor);
            var func = lambda.Compile();
            var entity = func.Invoke();
            return entity;
        }
    }
}