using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    /// <summary>
    /// Класс обрабатывает данные ADO.Net Provider'a  если запрос мапится в конкретные или анонимные классы
    /// </summary>
    [Info("Optimize code")]
    public class ComplexObjectBinder
    {
        private readonly IClassBinder _entityBuilder;

        public ComplexObjectBinder(IClassBinder entityBuilder)
        {
            _entityBuilder = entityBuilder;
        }


        // UsedImplicitly //
        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var fields = dataReader.FieldCount;
            var list = new List<T>();
            PropertyInfo[] props;
            var names = Enumerable.Range(0, fields).Select(dataReader.GetName).ToArray();
            props = SortPropertiesByColumnName(typeof(T).GetProperties().ToArray(), names);
            var s = new Stopwatch();
            do
            {
                var values = new object[fields];
                while (dataReader.Read())
                {
                    for (int i = 0; i < names.Length; i++)
                        values[i] = dataReader[names[i]];
                    list.Add(_entityBuilder.Handle<T>(
                        new PropertiesNameValues() {Properties = props, Values = values}));
                }
            } while (dataReader.NextResult());

            return list.GetEnumerator();
        }

        /// <summary>
        ///  метод сортирует приходящие из dataReader'a имена колонок в соответсвии со свойствами
        /// </summary>
        /// <returns></returns>
        private PropertyInfo[] SortPropertiesByColumnName(PropertyInfo[] propertyInfos, string[] columnNames)
        {
            var props = new PropertyInfo[columnNames.Length];
            for (int i = 0; i < propertyInfos.Length; i++)
            for (int j = 0; j < columnNames.Length; j++)
                if (propertyInfos[i].Name == columnNames[j])
                    props[j] = propertyInfos[i];
            return props;
        }
    }
}