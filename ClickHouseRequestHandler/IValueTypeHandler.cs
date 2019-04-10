using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface IPrimitiveTypeOrPrimitiveArrayBinder
    {
        T Handle<T>(NameValue nameValue);
    }


    public interface IClassBinder
    {
        T Handle<T>(PropertiesNameValues cells);
    }
   
}