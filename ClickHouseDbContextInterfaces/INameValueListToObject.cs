using System.Collections.Generic;

namespace DbContext
{
    public interface INameValueListToObject
    {
        T Build<T>(IEnumerable<NameValue> cells);
    }
}