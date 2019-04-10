using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClickDbContextInfrastructure
{
    public interface IMergeTreeDbSetOperations<T>
    {
        void Add(IEnumerable<T> items);
        void Remove(Expression<Func<T, bool>> exprFilter, Expression dbSetInitialExpression);

    }
}