using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbContext
{
    public class PrimitiveTypeCreator : IPrimitiveTypeOrPrimitiveArrayBinder
    {
        public T Handle<T>(NameValue nameValue) => (T) Convert.ChangeType(nameValue.Value, typeof(T));
    }
}