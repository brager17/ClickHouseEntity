using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace ExpressionTreeVisitor
{
    // marker
    public interface IHasKey<T> : IHasKey
    {
        T Key { get; set; }
    }

    public interface IHasKey
    {
        object ObjectKey { get; set; }
    }

    public interface IHasKeys<T>
    {
        IEnumerable<T> Names { get; set; }
    }

    public class NameAttribute : Attribute, IHasKey<string>
    {
        public string Key { get; set; }

        public NameAttribute(string name)
        {
            Key = name;
            ObjectKey = name;
        }


        public object ObjectKey { get; set; }
    }

    public abstract class NamesAttribute : Attribute, IHasKeys<string>
    {
        protected NamesAttribute(string[] names)
        {
            Names = names;
        }

        public IEnumerable<string> Names { get; set; }
    }

    public class InfoAttribute : NameAttribute
    {
        public string Name { get; set; }

        public InfoAttribute(string name) : base(name)
        {
            Name = name;
        }
    }

    public static class IHasNameExtensions
    {
        public static string GetNameAttributeValueEnumMember(this Enum @enum)
        {
            var enumType = @enum.GetType();
            var enumMember = enumType.GetMember(@enum.ToString()).Single();
            var attribute = enumMember.GetCustomAttribute<NameAttribute>();
            var name = attribute?.Key ?? enumMember.Name;
            return name;
        }


        public static TKey GetNameAttributeValueEnumMember<TKey>(this Type type, string AttributeName = null)
        {
            if (!type.IsClassType())
                throw new ArgumentException("Type должен быть классом");

            var values = GetKeyByType<TKey>(type.GetCustomAttributes(), AttributeName);
            return values.SingleOrDefault();
        }

        private static IEnumerable<TKey> GetKeyByType<TKey>(IEnumerable<Attribute> attributes, string AttributeName)
        {
            var s = typeof(TKey);
            bool FilterFunc(Attribute x) => typeof(IHasKey<>).MakeGenericType(typeof(TKey)).IsInstanceOfType(x);
            var values = attributes
                .Where(x => FilterFunc(x) && (AttributeName == null || x.GetType().Name == AttributeName))
                .Cast<IHasKey>()
                .Select(x => x.ObjectKey)
                .ToList();

            return values.Cast<TKey>();
        }

        public static TKey GetAttributeKeyByPropertyInfo<TKey>(this PropertyInfo propInfo,
            string AttributeName = null) => GetKeyByType<TKey>(propInfo.GetCustomAttributes(), AttributeName).Single();

        public static IEnumerable<TKey> GetPropertiesAttributesByClass<TKey>(this Type type,
            string AttributeName = null)
        {
            if (!type.IsClassType())
                throw new ArgumentException("Type должен быть классом");
            return type.GetProperties().SelectMany(x => GetKeyByType<TKey>(x.GetCustomAttributes(), AttributeName));
        }
    }
}