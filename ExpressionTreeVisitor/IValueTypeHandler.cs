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
        T Handle<T>(PropertiesNameValues cells);
    }

    public interface IManyObjectBinder
    {
        T Handle<T>(IEnumerable<NameValue> nameValue);
    }
}