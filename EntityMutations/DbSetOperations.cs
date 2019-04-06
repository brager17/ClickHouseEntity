using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClickDbContextInfrastructure;

namespace EntityTracking
{
    public class DbSetOperations<T> : IDbSetOperations<T>
    {
        public Task Add(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public Task Remove(IEnumerable<T> item, IEnumerator<T> enumerator)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Expression<Func<T, bool>> exprFilter)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}