using System;

namespace ClickHouseTableGenerator
{
    /// <summary>
    /// не создавать таблицу в бд
    /// </summary>
    public class NoCreateTable : Attribute
    {
    }
}