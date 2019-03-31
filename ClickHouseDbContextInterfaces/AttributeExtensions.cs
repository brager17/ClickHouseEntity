using System.Linq;
using System.Reflection;

namespace DbContext
{
    public static class AttributeExtensions
    {
        public static string GetColumnName(this PropertyInfo propertyInfo) =>
            propertyInfo.GetCustomAttributes<ColumnNameAttribute>().SingleOrDefault()?.Name ?? propertyInfo.Name;
    }
}