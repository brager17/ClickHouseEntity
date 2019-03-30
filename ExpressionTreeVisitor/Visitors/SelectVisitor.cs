using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeVisitor
{
    public interface IExpressionToLinqInfoHandler<TInfo>
    {
        TInfo GetLinqInfo<T>(Expression<T> expression);
    }

    class DtoToExpressionToLinqInfoHandler : IExpressionToLinqInfoHandler<SelectInfo>
    {
        public SelectInfo GetLinqInfo<T>(Expression<T> expression)
        {
            var t = expression.Parameters;
            var selectInfo = new SelectInfo {LambdaType = expression.Parameter().Type};
            var list = new List<KeyValuePair<PropertyInfo, InPropertySelectExpression>>();

            //todo refact
            if (expression.Body is NewExpression newExpression)
            {
                var props = newExpression.Members.Cast<PropertyInfo>();
                var ctorMembersExpressions = newExpression.Arguments.ToList().Cast<MemberExpression>();
                list.AddRange(
                    props.Zip(ctorMembersExpressions, (x, y) => new {anonymPropType = x, memberExpression = y})
                        .Select(x =>
                            new KeyValuePair<PropertyInfo, InPropertySelectExpression>(
                                x.anonymPropType, new InPropertySelectExpression
                                {
                                    _memberExpression = x.memberExpression
                                })));
            }
            else if (expression.Body is MemberInitExpression memberInitExpression)
                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    if (!(memberBinding is MemberAssignment assignment) ||
                        !(assignment.Expression is MemberExpression memberExpression))
                        throw new NotSupportedException();

                    var keyValue = new KeyValuePair<PropertyInfo, InPropertySelectExpression>(
                        (PropertyInfo) assignment.Member,
                        new InPropertySelectExpression {_memberExpression = memberExpression});
                    list.Add(keyValue);
                }
            else throw new NotSupportedException();

            list.ForEach(x => selectInfo._propertyExpressions.Add(x.Key, x.Value));
            return selectInfo;
        }
    }

    public class ValueTypeExpressionToLinqInfoHandler : IExpressionToLinqInfoHandler<SelectInfo>
    {
        public SelectInfo GetLinqInfo<T>(Expression<T> expression)
        {
            var selectInfo = new SelectInfo {LambdaType = expression.Parameter().Type};
            var propertyExpressionInfos = new List<KeyValuePair<PropertyInfo, InPropertySelectExpression>>();

            if (!(expression is LambdaExpression lambdaExpression &&
                  lambdaExpression.Body is MemberExpression memberExpression))
                throw new NotSupportedException();

            var keyValuePair = new KeyValuePair<PropertyInfo, InPropertySelectExpression>(
                (PropertyInfo) memberExpression.Member,
                new InPropertySelectExpression
                {
                    _memberExpression = memberExpression
                });
            propertyExpressionInfos.Add(keyValuePair);

            propertyExpressionInfos.ForEach(x => selectInfo._propertyExpressions.Add(x.Key, x.Value));
            return selectInfo;
        }
    }

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
            else if (IsConcreteOrAnonymouseClass(fromType))
                SelectInfo = _dtoToLinqInfoHandler.GetLinqInfo(node);
            else
                throw new NotSupportedException();
            return base.VisitLambda(node);
        }

        public SelectInfo SelectInfo { get; set; }
        private Boolean IsConcreteOrAnonymouseClass(Type t) => t.GetProperties().Any();
    }
}