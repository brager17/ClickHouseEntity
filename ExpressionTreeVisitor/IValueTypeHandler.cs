using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface IEntityTypeHandler
    {
        IEnumerator<T> Handle<T>(IDataReader dataReader);
    }

  
}