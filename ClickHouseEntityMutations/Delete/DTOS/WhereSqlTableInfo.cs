using System;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    public class WhereSqlTableInfo
    {
        public WhereStr WhereStr { get; set; }
        public Type TableType { get; set; }
    }
}