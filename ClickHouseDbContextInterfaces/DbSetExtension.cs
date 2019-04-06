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

        private static IEnumerable<Type> DbSetTypes => _dbSetTypes ?? (_dbSetTypes = 
                                                           AppDomain.CurrentDomain
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
    }
}