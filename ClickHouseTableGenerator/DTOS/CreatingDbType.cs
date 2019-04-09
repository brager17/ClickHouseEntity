namespace ClickHouseTableGenerator
{
    /// <summary>
    /// Enum определяющий сопоставление c# типов и типов данных CLickHouse
    /// </summary>
    public enum CreatingDbType
    {
        [DbType("Int32")] Int32,
        [DbType("UInt32")] UInt32,
        [DbType("Double")] Double,
        [DbType("Float")] Float,
        [DbType("String")] String,
        [DbType("Boolean")] Boolean,
        [DbType("Char")] Char,
        [DbType("DateTime")] DateTime,
        [DbType("DateTime")] UInt64,
        [DbType("Array(UInt32")] ArrayUInt32,
        [DbType("Array(UInt32)")] ArrayInt32
    }
}