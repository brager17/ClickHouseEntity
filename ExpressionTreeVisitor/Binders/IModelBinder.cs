using System.Data;
using System.Linq.Expressions;

namespace DbContext
{
    public interface IModelBinder
    {
        T Bind<T>(IDataReader reader);
    }
}