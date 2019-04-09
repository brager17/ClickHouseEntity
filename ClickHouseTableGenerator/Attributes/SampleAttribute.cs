using System;

namespace ClickHouseTableGenerator
{
    /// <summary>
    /// ключи по которым нужно нужно сэмплировать
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)] public class SampleAttribute : Attribute
    {
    }
}