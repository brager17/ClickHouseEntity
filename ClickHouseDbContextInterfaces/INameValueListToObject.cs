using System.Collections.Generic;

namespace DbContext
{
    public interface INameValueListToObject
    {
        // invoke 
        T Build<T>(IEnumerable<NameValue> cells);
    }
}