using System;
using System.Collections.Generic;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;
using Context;
using DbContext;
using EntityTracking;

namespace Root
{
    public static class GetDbSet
    {
        private static List<Condition<Type, IQuery<object, string>>> GetConditionQuery()
        {
            return new List<Condition<Type, IQuery<object, string>>>
            {
                new Condition<Type, IQuery<object, string>>
                {
                    _predicate = x => x == typeof(string),
                    _selector = new ConditionQueryFactoryAdapter(new StringTransform())
                },
                new Condition<Type, IQuery<object, string>>()
                {
                    _predicate = x => x == typeof(DateTime),
                    _selector = new ConditionQueryFactoryAdapter(new DateTimeTransform())
                },
                new Condition<Type, IQuery<object, string>>()
                {
                    _predicate = x => true,
                    _selector = new ConditionQueryFactoryAdapter(new DefaultTransform())
                }
            };
        }

        //todo инкапсулировать connection string в одном классе
        public static IDbSetOperations<T> GetDbSetOperations<T>(string connectionString, IEnumerable<IDbLogger> loggers)
        {
            return new DbSetOperations<T>(
                new InsertHandler<T>(
                    new InsertInfoQuery<T>(),
//                    new LoggerDecaoratorToSqlConverter<InsertInfo>(
                    new InsertInfoToSqlString(),
                    /*loggers*/
                    new InsertHandler(connectionString)));
//                )
        }
    }
}