using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;

namespace EntityTracking
{
    public class DbSetOperations<T> : IDbSetOperations<T>
    {
        private readonly IHandler<IEnumerable<T>> _addEnumerableHandler;

        public DbSetOperations(IHandler<IEnumerable<T>> addEnumerableHandler)
        {
            _addEnumerableHandler = addEnumerableHandler;
        }

        public void Add(IEnumerable<T> items)
        {
            _addEnumerableHandler.Handle(items);
        }

        public void Remove(IEnumerable<T> item, IEnumerator<T> enumerator)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<T, bool>> exprFilter)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}