using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpressionTreeVisitor;

namespace DbContext
{
    //todo добавить кэширование MethodInfo
    public class ClassTypeBinder : IEntityTypeBinder
    {
        private readonly IRowToObject _concreteClassRowToObject;
        private readonly IRowToObject _anonymousClassRowToObject;

        public ClassTypeBinder(IRowToObject concreteClassRowToObject, IRowToObject anonymousClassRowToObject)
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
}