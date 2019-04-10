using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ExpressionsToObject : IExpressionsToObject
    {
        private readonly IQuery<ForSqlRequestInfo, string> _toSqlConverter;
        private readonly IDbHandler _dbHandler;
        private readonly IQuery<Expression, AggregateLinqInfo> _visitorHandler;

        public ExpressionsToObject(
            IQuery<ForSqlRequestInfo, string> toSqlConverter,
            IDbHandler dbHandler, IQuery<Expression, AggregateLinqInfo> visitorHandler)
        {
            _toSqlConverter = toSqlConverter;
            _dbHandler = dbHandler;
            _visitorHandler = visitorHandler;
        }

        //TResult type definition IEnumerator<T> where T DbSet type 
        public TResult Handle<TResult>(Expression expression)
        {
            var aggregateInfo = _visitorHandler.Query(expression);
            var sql = _toSqlConverter.Query(aggregateInfo);
            var enumerator = _dbHandler.GetData<TResult>(sql);
            return enumerator;
        }
    }
}