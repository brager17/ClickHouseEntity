using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbContext
{
    public class ValueTypeBinder : IEntityTypeBinder
    {
        public IEnumerator<T> Handle<T>(IDataReader dataReader)
        {
            var arr = new List<object>();
            var name = dataReader.GetName(0);
            do
                while (dataReader.Read())
                    arr.Add(dataReader[name]);
            while (dataReader.NextResult());
            
            var result = arr.Select(x => (T) Convert.ChangeType(x, typeof(T)));
            return result.GetEnumerator();
        }
    }
}