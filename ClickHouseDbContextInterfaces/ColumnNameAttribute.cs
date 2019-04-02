using System;
using ExpressionTreeVisitor;

namespace DbContext
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : NameAttribute
    {
        public ColumnNameAttribute(string columnName) : base(columnName)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : NameAttribute
    {
        public TableNameAttribute(string tableName) : base(tableName)
        {
        }
    }
}