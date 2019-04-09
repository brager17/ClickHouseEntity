using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    public static class AttributeExtensions
    {
        public static string GetColumnName(this PropertyInfo propertyInfo) =>
            propertyInfo.GetCustomAttributes<ColumnKeyAttribute>().SingleOrDefault()?.Key ?? propertyInfo.Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">тип класса</param>
        /// <returns></returns>
        public static IEnumerable<string> GetColumnNameByAttributeOnProperty<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            if (!type.IsClassType()) throw new ArgumentException("Аргумент должен быть классом");
            return type.GetProperties().Where(x => x.GetCustomAttribute<TAttribute>() != null)
                .Select(x => x.GetColumnName());
        }

        public static object GetDefaultValue(this Type type) => type.GetCustomAttribute<DefaultValueAttribute>()?.Value;
    }
}