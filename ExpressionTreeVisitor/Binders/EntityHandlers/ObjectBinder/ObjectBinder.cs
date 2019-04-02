using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbContext
{
    public class ObjectBinder
    {
        private readonly IComplexEntityBinder _entityBuilder;

        public ObjectBinder(IComplexEntityBinder entityBuilder)
        {
            _entityBuilder = entityBuilder;
        }

        // UsedImplicitly //
        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var list = new List<T>();
            var fields = dataReader.FieldCount;

            var names = Enumerable.Range(0, fields).Select(dataReader.GetName).ToList();
            do
                while (dataReader.Read())
                    list.Add(_entityBuilder.Handle<T>(names
                        .Select(name => new NameValue {Value = dataReader[name], Name = name}).ToList()));
            while (dataReader.NextResult());

            return list.GetEnumerator();
        }
    }
}