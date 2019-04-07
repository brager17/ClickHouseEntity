using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;
using Context;
using DbContext;
using EntityTracking;
using EntityTracking.Delete;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetDbSet
    {
        //todo инкапсулировать connection string в одном классе
        public static IDbSetOperations<T> GetDbSetOperations<T>(string connectionString, IEnumerable<IDbLogger> loggers)
        {
            return new DbSetOperations<T>(
                new StopWatchHandler<IEnumerable<T>>(
                    new InsertHandler<T>(
                        new StopWatchQuery<T[], InsertInfo>(
                            new InsertInfoQuery<T>(
                                new CacheQueryWithHashCodeDto<TypeInfo, Func<T, object[]>>(
                                    new Aggregate2Query<TypeInfo, Expression<Func<T, object[]>>, Func<T, object[]>>(
                                        new ObjectArrayLambdaCreator<T>(),
                                        new LambdaCompileQuery<Func<T, object[]>>())
                                ), new TableNameRequestHandler()), loggers),
                        new StopWatchQuery<InsertInfo, AddingSql>(
                            new LoggerDecoratorWithConverter<InsertInfo, AddingSql>(
                                new InsertInfoToSqlString(),
                                new AddingSqlLoggingConverter(), loggers), loggers),
                        new StopWatchHandler<AddingSql>(
                            new WriteDbHandler<AddingSql>(connectionString, new DbInsertMutableQuery()), loggers)
                    ), loggers),
                new DeleteEnumerableHandler<T>(
                    new PropertyMapInfoVisitor(
                        new DtoToExpressionToLinqInfoHandler(),
                        new ValueTypeExpressionToLinqInfoHandler()),
                    new WhereToSqlVisitor(),
                    new LoggerDecoratorWithConverter<WhereSqlTableInfo, DeleteStr>(
                        new GetDeleteSql(new TableNameRequestHandler()), new DeleteStrLoggingConverter(), loggers),
                    new GetPropsByMemberFactory(),
                    new WriteDbHandler<HasSqlStringInfo>(connectionString, new StubMutableQuery())));
        }
    }
}