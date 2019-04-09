using System;
using System.Collections.Generic;
using ClickHouseDbContextExntensions.CQRS;

namespace ClickHouseTableGenerator
{
    /// <summary>
    ///  маппинг типов c# в типы ClickHouse
    /// </summary>
    public class DbTypeToCSharp : IQuery<Type, CreatingDbType>
    {
        private Dictionary<Type, CreatingDbType> _dic = new Dictionary<Type, CreatingDbType>
        {
            {typeof(int), CreatingDbType.Int32},
            {typeof(long), CreatingDbType.UInt32},
            {typeof(float), CreatingDbType.Float},
            {typeof(double), CreatingDbType.Double},
            {typeof(string), CreatingDbType.String},
            {typeof(bool), CreatingDbType.Boolean},
            {typeof(DateTime), CreatingDbType.DateTime},
            {typeof(ulong), CreatingDbType.UInt64},
            {typeof(long[]),CreatingDbType.ArrayUInt32},
            {typeof(int[]),CreatingDbType.ArrayInt32},
        };

        public CreatingDbType Query(Type input)
        {
            if (_dic.TryGetValue(input, out var value))
                return value;
            throw new NotImplementedException();
        }
    }
}