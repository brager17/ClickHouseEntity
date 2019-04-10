using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClickDbContextInfrastructure
{
    public interface IMergeTreeDbSet<T>
    {
        /// <summary>
        /// Добавление данных
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        void Add(IEnumerable<T> items);

        /// <summary>
        /// Удаление по данных по условию
        /// </summary>
        /// <param name="exprFilter">Условие удаления данных</param>
        /// <returns></returns>
        void Remove(Expression<Func<T, bool>> exprFilter);

    }
}