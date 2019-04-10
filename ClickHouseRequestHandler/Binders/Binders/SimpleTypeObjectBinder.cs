using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    /// <summary>
    /// Класс обрабатывает данные ADO.Net Provider'a  если запрос мапится в примитивный тип
    /// </summary>
    public class SimpleTypeObjectBinder
    {
        private readonly IPrimitiveTypeOrPrimitiveArrayBinder _entityBuilder;

        public SimpleTypeObjectBinder(IPrimitiveTypeOrPrimitiveArrayBinder entityBuilder)
        {
            _entityBuilder = entityBuilder;
        }

        // UsedImplicitly //
        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var list = new List<T>();
            var name = dataReader.GetName(0);
            do
            {
                while (dataReader.Read())
                {
                    list.Add(_entityBuilder.Handle<T>(new NameValue {Value = dataReader[name]}));
                }
            } while (dataReader.NextResult());

            return list.GetEnumerator();
        }
    }
}