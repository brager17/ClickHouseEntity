using System;
using System.Collections.Generic;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class SqlRequestHandler : ISqlRequestHandler
    {
        private readonly IOperationRequestHandle<IEnumerable<SelectInfo>> _selectRequestHandler;
        private readonly IOperationRequestHandle<Type> _tableNameHandler;
        private readonly IOperationRequestHandle<IEnumerable<WhereInfo>> _whereRequestHandler;

        public SqlRequestHandler(
            IOperationRequestHandle<IEnumerable<SelectInfo>> selectRequestHandler,
            IOperationRequestHandle<Type> tableNameHandler,
            IOperationRequestHandle<IEnumerable<WhereInfo>> whereRequestHandler)
        {
            _selectRequestHandler = selectRequestHandler;
            _tableNameHandler = tableNameHandler;
            _whereRequestHandler = whereRequestHandler;
        }

        public SqlRequest Handle(ForSqlRequestInfo linqInfo) => new SqlRequest()
        {
            Select = _selectRequestHandler.Handle(linqInfo.SelectInfo),
            TableName = _tableNameHandler.Handle(linqInfo.SetType),
            Where = _whereRequestHandler.Handle(linqInfo.WhereInfo)
        };
    }
}