using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface IEntityTypeBinder
    {
        IEnumerator<T> Handle<T>(IDataReader dataReader);
    }

  
}