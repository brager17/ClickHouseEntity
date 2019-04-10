using System.Data;
using System.Linq.Expressions;

namespace DbContext
{
    public interface IDataHandler
    {
        T Handle<T>(IDataReader reader);
    }
}