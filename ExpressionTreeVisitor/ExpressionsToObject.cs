using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class ExpressionsToObject : IExpressionsToObject
    {
        private readonly IExpressionToSqlConverter _toSqlConverter;
        private readonly IDbHandler _dbHandler;
        private readonly IVisitorHandler _visitorHandler;

        public ExpressionsToObject(
            IExpressionToSqlConverter toSqlConverter,
            IDbHandler dbHandler, IVisitorHandler visitorHandler)
        {
            _toSqlConverter = toSqlConverter;
            _dbHandler = dbHandler;
            _visitorHandler = visitorHandler;
        }

        //TResult type definition IEnumerator<T> where T DbSet type 
        public TResult Handle<TResult>(Expression expression)
        {
            var aggregateInfo = _visitorHandler.Handle(expression);
            var sql = _toSqlConverter.GetSql(aggregateInfo);
            var enumerator = _dbHandler.GetData<TResult>(sql);
            return enumerator;
        }
    }
}