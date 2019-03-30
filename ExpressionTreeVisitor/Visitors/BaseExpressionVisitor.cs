using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using ClickHouse.Ado;
using DbContext;

namespace ExpressionTreeVisitor
{
    public interface IGetBindingInfo
    {
        BindInfo GetBindInfo(OrderingAggregateLinqInfo orderingAggregate);
    }


    public class GetBindingInfo : IGetBindingInfo
    {
        //todo возможно стоит посылать только IEnumerable<SelectInfo>
        public BindInfo GetBindInfo(OrderingAggregateLinqInfo orderingAggregateLinqInfo)
        {
            var selectInfos = orderingAggregateLinqInfo.SelectInfo.ToList();
            selectInfos.Reverse();
            if (!selectInfos.Last()._propertyExpressions.Any())
                throw new NotSupportedException("В последнем Select должна быть проекция в свойства");
            var destTypeInfo = selectInfos.Last()._propertyExpressions.FirstOrDefault().Key.DeclaringType;
            var selectParameters = Enumerable.Empty<PropertyInfo>();
            var destProperties = Enumerable.Empty<PropertyInfo>();
            if (selectInfos.Last().LambdaType.IsClass)
            {
                destProperties = selectInfos.Last()._propertyExpressions.Select(x => x.Key);
                if (selectInfos.Count == 1)
                {
                    selectParameters = selectInfos.Last()
                        ._propertyExpressions.Select(x => (PropertyInfo) x.Value._memberExpression.Member);
                }
                else
                    selectParameters = GetSourcePropertyInfo(destProperties, selectInfos.SkipLast(1));
            }

            else if (selectInfos.Last().IsPrimitiveSelect)
            {
                destProperties = new[] {selectInfos.Last()._propertyExpressions.Single().Key};
                selectParameters = GetSourcePropertyInfo(destProperties, selectInfos.SkipLast(1));
            }

            return new BindInfo()
            {
                DestType = destTypeInfo,
                BindMemberInfos = selectParameters.Select(x => new BindMemberInfo
                {
                    ColumnName = x.GetColumnName(),
                    DestPropName = x.Name
                })
            };
        }


        private IEnumerable<PropertyInfo> GetSourcePropertyInfo(IEnumerable<PropertyInfo> destProps,
            IEnumerable<SelectInfo> infos)
        {
            var names = destProps.Select(x => x.Name).ToList();
            var props = new List<PropertyInfo>();
            foreach (var info in infos)
            {
                var s = info._propertyExpressions.Where(x => names.Contains(x.Key.Name)).ToList();
                names = s.Select(x => x.Value._memberExpression.Member.Name).ToList();
                props = s.Select(x => (PropertyInfo) x.Value._memberExpression.Member).ToList();
            }

            return props;
        }
    }

    public class BaseExpressionVisitor : ExpressionVisitor
    {
        private readonly IGetBindingInfo _getBindingInfo;
        private SelectVisitor SelectVisitor { get; set; }
        private OrderingAggregateLinqInfo AggregateLinqInfo { get; set; }

        public BaseExpressionVisitor(IGetBindingInfo getBindingInfo)
        {
            _getBindingInfo = getBindingInfo;
        }


        public AggregateLinqInfo GetInfo(Expression expression)
        {
            SelectVisitor = new SelectVisitor(new DtoToExpressionToLinqInfoHandler(),
                new ValueTypeExpressionToLinqInfoHandler());
            AggregateLinqInfo = new OrderingAggregateLinqInfo();
            base.Visit(expression);
            AggregateLinqInfo.BindInfo = _getBindingInfo.GetBindInfo(AggregateLinqInfo);
            return AggregateLinqInfo;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Select") AggregateLinqInfo.SelectInfo.Add(SelectVisitor.GetInfo(node));

            return base.VisitMethodCall(node);
        }

        //todo добавить более корректную проверку, что ConstantExpression это именно ConstantExpression DbSet'а
        protected override Expression VisitConstant(ConstantExpression node)
        {
            AggregateLinqInfo.SetType = node.Type.GetGenericArguments().Single();
            return base.VisitConstant(node);
        }

        private bool IsLastSelect() => AggregateLinqInfo.BindInfo?.DestType == null;
    }
}