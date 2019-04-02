using System;
using System.Linq;
using DbContext;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetClickHouseProvider
    {
        public static ExpressionsToObject Get(string ConnectionString) =>
            new ExpressionsToObject(
                new ExpressionToSqlConverter(new SqlRequestHandler(
                        new SelectRequestHandler(),
                        new TableNameRequestHandler(),
                        new WhereOperationRequestHandler(),
                        new TakeOperationRequestHandle()),
                    new BaseExpressionVisitor()),
                new DbHandler(ConnectionString,
                    new ModelBinder(
                        new ObjectBinder(new SimpleTypeBuilderAdapter(new ValueTypeBinder())),
                        new ObjectBinder(
                            new ClassTypeBinder(new ConcreteClassNameValueListToObject(),
                                new AnonymousClassNameValueListToObject())))), new BaseExpressionVisitor());
    }
}