using System;
using Root;

namespace ClickHouseTableGenerator
{
    public interface ITableGenerator
    {
        void Generate(ClassType dbSetType);
    }
}