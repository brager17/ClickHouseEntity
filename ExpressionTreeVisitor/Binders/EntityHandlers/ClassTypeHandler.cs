using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ClassTypeHandler : IEntityTypeHandler
    {
        private readonly IRowToObject _concreteClassRowToObject;
        private readonly IRowToObject _anonymousClassRowToObject;

        public ClassTypeHandler(IRowToObject concreteClassRowToObject, IRowToObject anonymousClassRowToObject)
        {
            _concreteClassRowToObject = concreteClassRowToObject;
            _anonymousClassRowToObject = anonymousClassRowToObject;
        }

        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var arr = new List<IEnumerable<Cell>>();
            var fields = dataReader.FieldCount;
            var names = Enumerable.Range(0, fields).Select(dataReader.GetName).ToList();
            do
                while (dataReader.Read())
                    arr.Add(names.Select(name => new Cell {Value = dataReader[name], Alias = name}).ToList());
            while (dataReader.NextResult());


            IRowToObject rowToObjectType;

            if (typeof(T).IsAnonymouseClass())
                rowToObjectType = _anonymousClassRowToObject;
            else
                rowToObjectType = _concreteClassRowToObject;

            var result = arr.Select(x => rowToObjectType.GetType().GetMethod("Build").MakeGenericMethod(typeof(T))
                .Invoke(rowToObjectType, new object[] {x})).ToList().Cast<T>();

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
        T Build<T>(IEnumerable<Cell> cells);
    }

    public class AnonymousClassRowToObject : IRowToObject
    {
        public T Build<T>(IEnumerable<Cell> cells)
        {
            var props = typeof(T).GetProperties();
            // строим констуктор
            var cellProps = props.Join(cells, x => x.Name, x => x.Alias,
                (x, y) => new {type = x.PropertyType, value = y.Value});
            var constantExpressions =
                cellProps.Select(x => Expression.Convert(Expression.Constant(x.value), x.type));
            var ctor = typeof(T).GetConstructors().Single();
            var expressionNew = Expression.New(ctor, constantExpressions);
            var ctorLambda = Expression.Lambda<Func<T>>(expressionNew);
            return ctorLambda.Compile().Invoke();
        }
    }


// todo adding cache
    public class ConcreteClassRowToObject : IRowToObject
    {
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