using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ClickDbContextInfrastructure
{
    public interface IDbSet
    {
    }

    public interface IDbSet<T>
    {
        /// <summary>
        /// Добавление данных
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        void Add(IEnumerable<T> items);

        /// <summary>
        /// Удаление списка элементов, если важна производительность используйте Remove(Expression<Func<T,bool>>)
        /// </summary>
        /// <returns></returns>
        void Remove(IEnumerable<T> item);

        /// <summary>
        /// Удаление по данных по условию
        /// </summary>
        /// <param name="exprFilter">Условие удаления данных</param>
        /// <returns></returns>
        void Remove(Expression<Func<T, bool>> exprFilter);

        void SaveChanges();
    }

    public interface IDbSetOperations<T>
    {
        void Add(IEnumerable<T> items);
        void Remove(IEnumerable<T> item, IEnumerator<T> enumerator);
        void Remove(Expression<Func<T, bool>> exprFilter);

        void SaveChanges();
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