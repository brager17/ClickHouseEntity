using System;
using System.Collections.Generic;
using ExpressionTreeVisitor;

namespace DbContext
{
    public class SqlRequestHandler : ISqlRequestHandler
    {
        private readonly IOperationRequestHandle<IEnumerable<SelectInfo>> _selectRequestHandler;
        private readonly IOperationRequestHandle<IEnumerable<WhereInfo>> _whereRequestHandler;
        private readonly IOperationRequestHandle<TakeInfo> _takeSkipInfo;
        private readonly IOperationRequestHandle<IEnumerable<OrderInfo>> _orderRequestHandler;

        public SqlRequestHandler(
            IOperationRequestHandle<IEnumerable<SelectInfo>> selectRequestHandler,
            IOperationRequestHandle<IEnumerable<WhereInfo>> whereRequestHandler,
            IOperationRequestHandle<TakeInfo> takeSkipInfo,
            IOperationRequestHandle<IEnumerable<OrderInfo>> orderRequestHandler)
        {
            _selectRequestHandler = selectRequestHandler;
            _whereRequestHandler = whereRequestHandler;
            _takeSkipInfo = takeSkipInfo;
            _orderRequestHandler = orderRequestHandler;
        }

        //todo 1)чтобы добавить одну команду нужно: добавить свойство string в SqlRequest -
        //    2) создать новый хэндлер +
        // 3) добавить в конструктор этого класса интерфейс, который реализует хэндлер 
        // 4) проинициализировать в методе Handle добавленное свойство
        // все можно забыть, нигде ничего не сломается,если забудешь, todo рефакторинг
        public SqlRequest Handle(ForSqlRequestInfo linqInfo) => new SqlRequest()
        {
            Select = _selectRequestHandler.Handle(linqInfo.SelectInfo),
            TableName = linqInfo.SetType.GetClassAttributeKey<string>(),
            Where = _whereRequestHandler.Handle(linqInfo.WhereInfo),
            Take = _takeSkipInfo.Handle(linqInfo.TakeInfo),
            OrderBy = _orderRequestHandler.Handle(linqInfo.OrderInfo)
        };
    }
}