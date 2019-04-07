using System.Linq.Expressions;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    public class DeleteEnumerableHandler<T> : IHandler<DeleteInfo<T>>
    {
        private readonly IQuery<Expression, LinqInfoPropertiesMap> _getSelectInfoQuery;
        private readonly WhereToSqlVisitor _whereVisitorQuery;
        private readonly IQuery<WhereSqlTableInfo, DeleteStr> _getSqlQuery;
        private readonly GetPropsByMemberFactory _getPropsByMemberFactory;
        private readonly WriteDbHandler<HasSqlStringInfo> _writeDb;

        public DeleteEnumerableHandler(
            IQuery<Expression, LinqInfoPropertiesMap> getSelectInfoQuery,
            WhereToSqlVisitor whereVisitorQuery,
            IQuery<WhereSqlTableInfo, DeleteStr> getSqlQuery,
            GetPropsByMemberFactory getPropsByMemberFactory,
            WriteDbHandler<HasSqlStringInfo> writeDb)
        {
            _getSelectInfoQuery = getSelectInfoQuery;
            _whereVisitorQuery = whereVisitorQuery;
            _getSqlQuery = getSqlQuery;
            _getPropsByMemberFactory = getPropsByMemberFactory;
            _writeDb = writeDb;
        }

        public void Handle(DeleteInfo<T> input)
        {
            var selectInfoQuery = _getSelectInfoQuery.Query(input.DbSetInitialExpression).SelectInfos;
            var whereString = new WhereSqlTableInfo()
            {
                WhereStr = _whereVisitorQuery.Visit(_getPropsByMemberFactory.Create(selectInfoQuery),
                    new InWhereToSqlVisitorInfo {Expression = input.FilterExpression}),
                TableType = typeof(T)
            };
            var deleteStringInfo = _getSqlQuery.Query(whereString);
            _writeDb.Handle(deleteStringInfo);
        }
    }
}