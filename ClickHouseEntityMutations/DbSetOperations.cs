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
        private readonly IHandler<DeleteInfo<T>> _deleteEnumerableHandler;

        public DbSetOperations(
            IHandler<IEnumerable<T>> addEnumerableHandler,
            IHandler<DeleteInfo<T>> deleteEnumerableHandler)
        {
            _addEnumerableHandler = addEnumerableHandler;
            _deleteEnumerableHandler = deleteEnumerableHandler;
        }

        public void Add(IEnumerable<T> items)
        {
            _addEnumerableHandler.Handle(items);
        }

        public void Remove(IEnumerable<T> item, IEnumerator<T> enumerator)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<T, bool>> exprFilter, Expression dbSetInitialExpression)
        {
            _deleteEnumerableHandler.Handle(new DeleteInfo<T>
                {FilterExpression = exprFilter, DbSetInitialExpression = dbSetInitialExpression});
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}