using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ClickDbContextInfrastructure
{
    public interface IDbSet
    {
    }

    public interface IClickHouseDbContext
    {
    }

    public static class DbSetExtension
    {
        private static IEnumerable<Type> _dbSetTypes { get; set; }

        private static IEnumerable<Type> DbSetTypes => _dbSetTypes ?? (_dbSetTypes = AppDomain.CurrentDomain
                                                           .GetAssemblies().SelectMany(x => x.GetTypes())
                                                           .Where(x =>
                                                               typeof(IClickHouseDbContext)
                                                                   .IsAssignableFrom(x.BaseType))
                                                           .SelectMany(x => x.GetProperties()
                                                               .Select(xx => xx.PropertyType)
                                                               .Where(xx => xx.IsGenericType &&
                                                                            typeof(IDbSet).IsAssignableFrom(
                                                                                xx.GetGenericTypeDefinition())))
                                                           .Select(x => x.GenericTypeArguments.Single()));


        public static Boolean IsDbSetType(this Type type)
        {
            var isDbSetType = DbSetTypes.Any(x => x.Name == type.Name);
            return isDbSetType;
        }

        public static object GetCachedReflectedInfo()
        {
            var type = Type
                .GetType(
                    "System.Linq.CachedReflectionInfo,System.Linq.Queryable, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            var methods = type.GetMethods().ToList();
            var cachedReflectedInfo = Activator.CreateInstance(type);
            return cachedReflectedInfo;
        }
    }
}