using System;
using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    /// <summary>
    /// Класс обрабатывает данные ADO.Net Provider'a  если запрос мапится в примитивный массив
    /// </summary>
    // todo дублирование с SimpleTypeObjectBinder, убрать try catch, когда исправим косяки ClickHouseAdoNetProvider'a
    public class ArrayObjectBinder
    {
        private readonly IPrimitiveTypeOrPrimitiveArrayBinder _entityBuilder;

        public ArrayObjectBinder(IPrimitiveTypeOrPrimitiveArrayBinder entityBuilder)
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
                    // ClickHouseAdoNet иногда падает, когда селект в массив
                    try
                    {
                        list.Add(_entityBuilder.Handle<T>(new NameValue {Value = dataReader[name]}));
                    }
                    catch (Exception exception)
                    {
                    }
                }
            } while (dataReader.NextResult());

            return list.GetEnumerator();
        }
    }
}