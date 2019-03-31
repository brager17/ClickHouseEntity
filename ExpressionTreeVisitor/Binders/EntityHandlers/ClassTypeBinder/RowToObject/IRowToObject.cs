using System.Collections.Generic;

namespace DbContext
{
    public interface IRowToObject
    {
        T Build<T>(IEnumerable<Cell> cells);
    }
}