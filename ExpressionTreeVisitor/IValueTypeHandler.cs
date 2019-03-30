using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface IValueTypeHandler
    {
        IEnumerator<T> Handle<T>(IDataReader dataReader);
    }

    public interface IEntityTypeHandler
    {
        IEnumerator<T> Handle<T>(IDataReader dataReader,BindInfo bindInfo);

    }
}