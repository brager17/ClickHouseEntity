using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ComplexObjectBinder
    {
        private readonly IComplexEntityBinder _entityBuilder;

        public ComplexObjectBinder(IComplexEntityBinder entityBuilder)
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
            props = SortPropertiesByColumnName(typeof(T).GetProperties().ToArray(),
                names);

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

    public class ArrayObjectBinder
    {
        private readonly ISingleObjectBinder _entityBuilder;

        public ArrayObjectBinder(ISingleObjectBinder entityBuilder)
        {
            _entityBuilder = entityBuilder;
        }

        // UsedImplicitly //
        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var list = new List<T>();
            var objects = new List<object>();
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

    public class SimpleTypeObjectBinder
    {
        private readonly ISingleObjectBinder _entityBuilder;

        public SimpleTypeObjectBinder(ISingleObjectBinder entityBuilder)
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