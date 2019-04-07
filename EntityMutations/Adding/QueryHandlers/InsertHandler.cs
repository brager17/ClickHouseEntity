using System.Collections.Generic;
using System.Linq;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class InsertHandler<T> : IHandler<IEnumerable<T>>
    {
        private readonly IQuery<T[], InsertInfo> _insertInfoQuery;
        private readonly IQuery<InsertInfo, AddingSql> _insertToSqlQuery;
        private readonly IHandler<AddingSql> _insertHandler;

        public InsertHandler(
            IQuery<T[], InsertInfo> insertInfoQuery,
            IQuery<InsertInfo, AddingSql> insertToSqlQuery,
            IHandler<AddingSql> insertHandler)
        {
            _insertInfoQuery = insertInfoQuery;
            _insertToSqlQuery = insertToSqlQuery;
            _insertHandler = insertHandler;
        }

        public void Handle(IEnumerable<T> input)
        {
            var insertInfo = _insertInfoQuery.Query(input.ToArray());
            var addingSql = _insertToSqlQuery.Query(insertInfo);
            _insertHandler.Handle(addingSql);
        }
    }
}