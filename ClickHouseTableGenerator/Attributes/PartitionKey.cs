using System;

namespace ClickHouseTableGenerator
{
    /// <summary>
    /// ключи по которому будет осуществляться партиционирование
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PartitionKey : Attribute
    {
    }
}