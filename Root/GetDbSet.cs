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

        //todo инкапсулировать connection string в одном классе
        public static IDbSetOperations<T> GetDbSetOperations<T>(string connectionString, IEnumerable<IDbLogger> loggers)
        {
            return new DbSetOperations<T>(
                new InsertHandler<T>(
                    new InsertInfoQuery<T>(),
//                    new LoggerDecaoratorToSqlConverter<InsertInfo>(
                    new InsertInfoToSqlString(),
                    /*loggers*/
                    new DbInsertHandler(connectionString)));
//                )
        }
    }
}