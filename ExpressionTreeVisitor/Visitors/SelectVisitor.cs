using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    public class SelectVisitor : ExpressionVisitor, IGetInfo<SelectInfo>
    {
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _dtoToLinqInfoHandler;
        private readonly IExpressionToLinqInfoHandler<SelectInfo> _valueTypeToLinqInfoHandler;

        public SelectVisitor(
            IExpressionToLinqInfoHandler<SelectInfo> dtoToLinqInfoHandler,
            IExpressionToLinqInfoHandler<SelectInfo> valueTypeToLinqInfoHandler)
        {
            _dtoToLinqInfoHandler = dtoToLinqInfoHandler;
            _valueTypeToLinqInfoHandler = valueTypeToLinqInfoHandler;
        }

        public SelectInfo GetInfo(Expression expression)
        {
            SelectInfo = new SelectInfo();
            base.Visit(expression);
            return SelectInfo;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T).GetGenericTypeDefinition() != typeof(Func<,>))
                throw new NotSupportedException();
            var fromType = typeof(T).GetGenericArguments().Last();
            if (fromType.IsSimpleType())
                SelectInfo = _valueTypeToLinqInfoHandler.GetLinqInfo(node);
            else if (!fromType.IsSimpleType())
                SelectInfo = _dtoToLinqInfoHandler.GetLinqInfo(node);
            else
                throw new NotSupportedException();
            return base.VisitLambda(node);
        }

        public SelectInfo SelectInfo { get; set; }
        
    }
}