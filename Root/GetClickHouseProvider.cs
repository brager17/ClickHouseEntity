using System.Collections.Generic;
using Context;
using DbContext;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetClickHouseProvider
    {
        //todo add DI Container(хотя он тут нахуй не нужен
        public static ExpressionsToObject Get(string ConnectionString, IEnumerable<IDbLogger> loggers) =>
            new ExpressionsToObject(
                new LoggerDecaoratorToSqlConverter(
                    new ExpressionToSqlConverter(new SqlRequestHandler(
                            new SelectRequestHandler(),
                            new TableNameRequestHandler(),
                            new WhereOperationRequestHandler(),
                            new TakeOperationRequestHandle(),
                            new OrderOperationRequestHandle()),
                        new AggregateLinqVisitor(new WhereToSqlVisitor(new HasMapPropInfo()),
                            new PropertyMapInfoVisitor(new DtoToExpressionToLinqInfoHandler(),
                                new ValueTypeExpressionToLinqInfoHandler()),
                            new OrderingToSqlVisitor(new HasMapPropInfo()))), loggers),
                new DbHandler(ConnectionString, new DataHandler(
                    new ObjectBinder(new SimpleTypeBuilderAdapter(new ValueTypeBinder())),
                    new ObjectBinder(new ClassTypeBinder(new ConcreteClassNameValueListToObject(),
                        new AnonymousClassNameValueListToObject())))),
                new VisitorsHandler(
                    new PropertyMapInfoVisitor(new DtoToExpressionToLinqInfoHandler(),
                        new ValueTypeExpressionToLinqInfoHandler()), new AggregateLinqVisitor(
                        new WhereToSqlVisitor(new HasMapPropInfo()),
                        new PropertyMapInfoVisitor(new DtoToExpressionToLinqInfoHandler(),
                            new ValueTypeExpressionToLinqInfoHandler()),
                        new OrderingToSqlVisitor(new HasMapPropInfo()))));
    }
}