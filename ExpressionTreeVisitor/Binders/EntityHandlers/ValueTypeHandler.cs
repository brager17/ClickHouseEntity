using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbContext
{
    public class ValueTypeBinder : ISingleObjectBinder
    {
        public T Handle<T>(Cell cell) => (T) Convert.ChangeType(cell.Value, typeof(T));
    }
}