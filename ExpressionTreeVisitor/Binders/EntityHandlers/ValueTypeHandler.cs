using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbContext
{
    public class ValueTypeBinder : ISingleObjectBinder
    {
        public T Handle<T>(NameValue nameValue)
        {
            var changeType = Convert.ChangeType(nameValue.Value, typeof(T));
            var handle = (T) changeType;
            return handle;
        }
    }
}