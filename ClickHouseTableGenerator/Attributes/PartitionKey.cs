using System;

namespace ClickHouseTableGenerator
{
    /// <summary>
    /// ключ по которому будет осуществляться партиционирование
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PartitionKey : Attribute
    {
    }
}