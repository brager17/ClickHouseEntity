using System.Collections.Generic;
using System.Data;

namespace DbContext
{
    public interface ISingleObjectBinder
    {
        T Handle<T>(Cell cell);
    }


    public interface IComplexEntityBinder
    {
        T Handle<T>(IEnumerable<Cell> cells);
    }
}