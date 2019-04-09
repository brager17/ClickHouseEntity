using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace ExpressionTreeVisitor
{
    public static class HasKeyExtensions
    {
        /// <summary>
        /// возвращает ключ атрибута NameAttribute, который повешен на элемента Enum'a,
        /// если его нет то просто имя этого элемента
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetNameAttributeValueEnumMember(this Enum @enum)
        {
            var enumType = @enum.GetType();
            var enumMember = enumType.GetMember(@enum.ToString()).Single();
            var attribute = enumMember.GetCustomAttribute<NameAttribute>();
            var name = attribute?.Key ?? enumMember.Name;
            return name;
        }

        /// <summary>
        /// Возвращает ключ атрибута класса,реализующего IHasKey<TKey>
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <param name="AttributeName">Имя атрибута добавленного на класс типа type</param>
        /// <typeparam name="TKey">Generic интерфейса IHasKey<TKey></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Возвращает один ключ атрибута реализующего IHasKey<TKey> по PropertyType
        /// </summary>
        /// <param name="propInfo">PropertyType свойства на котором используется атрибут реализующий IHasKey<TKey></param>
        /// <param name="AttributeName"></param>
        /// <typeparam name="TKey">Generic интерфейса IHasKey<TKey></typeparam>
        /// <returns></returns>
        public static TKey GetAttributeKeyByPropertyInfo<TKey>(this PropertyInfo propInfo,
            string AttributeName = null) => GetKeyByType<TKey>(propInfo.GetCustomAttributes(), AttributeName).Single();

        /// <summary>
        /// Метод собирает все атрибуты реализующие интерфейс IHasKey<TKey> которые повешены на свойства класса типа type
        /// и возвращает их ключи
        /// </summary>
        /// <param name="type">Класс</param>
        /// <param name="AttributeName">Имя атрибута</param>
        /// <typeparam name="TKey">Generic интерфейса IHasKey<TKey></typeparam>
        /// <returns>все ключи атрибутов свойтв реализующих интефейс IHasKey<TKey></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<TKey> GetPropertiesAttributesByClass<TKey>(this Type type,
            string AttributeName = null)
        {
            if (!type.IsClassType())
                throw new ArgumentException("Type должен быть классом");
            return type.GetProperties().SelectMany(x => GetKeyByType<TKey>(x.GetCustomAttributes(), AttributeName));
        }
    }
}