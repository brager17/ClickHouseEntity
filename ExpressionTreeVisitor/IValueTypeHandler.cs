using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface ISingleObjectBinder
    {
        T Handle<T>(NameValue nameValue);
    }


    public interface IComplexEntityBinder
    {
        T Handle<T>(IEnumerable<NameValue> cells);
    }
}