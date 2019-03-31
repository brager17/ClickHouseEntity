using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace DbContext
{
    public interface IDbHandler
    {
        T GetData<T>(string sql);
    }
}