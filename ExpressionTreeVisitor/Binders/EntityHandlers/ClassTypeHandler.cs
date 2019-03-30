using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ClassTypeHandler : IEntityTypeHandler
    {
        private readonly IRowToObject _rowToObject;

        public ClassTypeHandler(IRowToObject rowToObject)
        {
            _rowToObject = rowToObject;
        }

        public IEnumerator<T> Handle<T>(IDataReader dataReader, BindInfo bindInfo)
        {
            var arr = new List<IEnumerable<Cell>>();
            var fields = dataReader.FieldCount;
            var names = Enumerable.Range(0, fields).Select(dataReader.GetName).ToList();
            do
                while (dataReader.Read())
                    arr.Add(names.Select(name => new Cell {Value = dataReader[name], Alias = name}).ToList());
            while (dataReader.NextResult());

            var buildMethodInfo = typeof(EntityRowToObject).GetMethod("Build").MakeGenericMethod(typeof(T));
            var result = arr.Select(x => buildMethodInfo.Invoke(_rowToObject, new object[] {x, bindInfo})).ToList()
                .Cast<T>();

            return result.GetEnumerator();
        }
    }

    public class Cell
    {
        public string Alias { get; set; }
        public object Value { get; set; }
    }

    public interface IRowToObject
    {
        T Build<T>(IEnumerable<Cell> cells, BindInfo bindInfo);
    }

    // todo adding cache
    public class EntityRowToObject : IRowToObject
    {
        public T Build<T>(IEnumerable<Cell> cells, BindInfo bindInfo)
        {
            var props = typeof(T).GetProperties();

            // строим констуктор
            if (bindInfo.DestType.IsAnonymouseClass())
            {
                var cellProps = props.Join(cells, x => x.Name, x => x.Alias,
                    (x, y) => new {type = x.PropertyType, value = y.Value});
                var constantExpressions =
                    cellProps.Select(x => Expression.Convert(Expression.Constant(x.value), x.type));
                var ctor = bindInfo.DestType.GetConstructors().Single();
                var expressionNew = Expression.New(ctor, constantExpressions);
                var ctorLambda = Expression.Lambda<Func<T>>(expressionNew);
                return ctorLambda.Compile().Invoke();
            }

            // через инициализатор

            var cellPropss = props.Join(cells, x => x.Name, x => x.Alias,
                (x, y) => new {propertyType = x, value = y.Value});
            var assignments = cellPropss.Select(x
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