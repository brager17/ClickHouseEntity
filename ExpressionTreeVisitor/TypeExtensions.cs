using System;
using System.Linq;

namespace ExpressionTreeVisitor
{
    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type) => type.IsPrimitive || type == typeof(string);


        // todo refact this
        public static bool IsAnonymouseClass(this Type type) => type.Name.Contains("AnonymousType");
    }
}