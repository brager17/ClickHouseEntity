using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    [Info("Optimize code")]
    public class InsertInfoQuery<T> : IQuery<T[], InsertInfo>
    {
        private readonly IQuery<TypeInfo, Func<T, object[]>> _createObjectArrayFunc;
        private readonly IOperationRequestHandle<Type> _getTableName;

        public InsertInfoQuery(
            IQuery<TypeInfo, Func<T, object[]>> _createObjectArrayFunc,
            IOperationRequestHandle<Type> getTableName)
        {
            this._createObjectArrayFunc = _createObjectArrayFunc;
            _getTableName = getTableName;
        }

        public InsertInfo Query(T[] rows)
        {
           
            var tableName = _getTableName.Handle(typeof(T));
            var properties = typeof(T).GetProperties().ToArray();
            var rowsLength = rows.Length;
            var values = new object[rowsLength][];
            var typeInfo = new TypeInfo() {Type = typeof(T)};

            for (int i = 0; i < rowsLength; i++)
            {
                values[i] = _createObjectArrayFunc.Query(typeInfo)(rows[i]);
            }

       
            var insertPropertiesName = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++) insertPropertiesName[i] = properties[i].GetColumnName();
            
            return new InsertInfo
            {
                Values = values, TableName = tableName, InsertColumns = insertPropertiesName
            };
        }
    }

    public class TypeInfo
    {
        public override bool Equals(object obj)
        {
            return ((TypeInfo) obj).Type.Name == Type.Name;
        }

        public override int GetHashCode()
        {
            if (Type == null)return 0;
            return Type.ToString().GetHashCode();
        }

        public Type Type { get; set; }
    }

    [Info("Optimize code")]
    public class ObjectArrayLambdaCreator<T> : IQuery<TypeInfo, Expression<Func<T, object[]>>>
    {
        public Expression<Func<T, object[]>> Query(TypeInfo input)
        {
            var properties = input.Type.GetProperties().ToArray();
            var parameter = Expression.Parameter(input.Type);
            var unaryExpressions = new UnaryExpression[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                unaryExpressions[i] = Expression.Convert(Expression.Property(parameter, properties[i]), typeof(object));
            var array = Expression.NewArrayInit(typeof(object), unaryExpressions);
            var lambda = Expression.Lambda<Func<T, object[]>>(array, parameter);
            return lambda;
        }
    }
}