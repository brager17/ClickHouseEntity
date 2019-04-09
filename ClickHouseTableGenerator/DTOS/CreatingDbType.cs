namespace ClickHouseTableGenerator
{
    public enum CreatingDbType
    {
        [CreatingDbTypeAttribute("Int32")] Int32,
        [CreatingDbTypeAttribute("UInt32")] UInt32,
        [CreatingDbTypeAttribute("Double")] Double,
        [CreatingDbTypeAttribute("Float")] Float,
        [CreatingDbTypeAttribute("String")] String,
        [CreatingDbTypeAttribute("Boolean")] Boolean,
        [CreatingDbTypeAttribute("Char")] Char,
        [CreatingDbTypeAttribute("DateTime")] DateTime,
        [CreatingDbTypeAttribute("DateTime")] UInt64,
        [CreatingDbTypeAttribute("Array(UInt32")] ArrayUInt32,
        [CreatingDbTypeAttribute("Array(UInt32)")] ArrayInt32
    }
}