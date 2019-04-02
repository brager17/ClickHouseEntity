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
        private readonly BaseExpressionVisitor _baseExpressionVisitor;

        public ExpressionsToObject(
            IExpressionToSqlConverter toSqlConverter,
            IDbHandler dbHandler, BaseExpressionVisitor baseExpressionVisitor)
        {
            _toSqlConverter = toSqlConverter;
            _dbHandler = dbHandler;
            _baseExpressionVisitor = baseExpressionVisitor;
        }

        //TResult type definition IEnumerator<T> where T DbSet type 
        public TResult Handle<TResult>(Expression expression)
        {
            var aggregateInfo = _baseExpressionVisitor.GetInfo(expression);
            var sql = _toSqlConverter.GetSql(aggregateInfo);
            var data = _dbHandler.GetData<TResult>(sql);
            return data;
        }
    }

  
}