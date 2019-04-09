using System;

namespace ClickHouseTableGenerator
{
    /// <inheritdoc />
    /// <summary>
    /// ключи сортировки
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderKey : Attribute
    {
    }
}