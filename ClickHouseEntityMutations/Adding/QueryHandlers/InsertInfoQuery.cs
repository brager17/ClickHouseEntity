using System;
using System.Diagnostics;
using System.Linq;
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

        public InsertInfoQuery(IQuery<TypeInfo, Func<T, object[]>> _createObjectArrayFunc)
        {
            this._createObjectArrayFunc = _createObjectArrayFunc;
        }

        public InsertInfo Query(T[] rows)
        {
            var tableName = typeof(T).GetNameAttributeValueEnumMember<string>();
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
}