using System;
using System.Linq;
using DbContext;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetClickHouseProvider
    {
        public static SqlToObject Get(string ConnectionString) =>
            new SqlToObject(
                new ExpressionToSqlConverter(new SqlRequestHandler(
                        new SelectRequestHandler(),
                        new TableNameRequestHandler()),
                    new BaseExpressionVisitor()),
                new DbHandler(ConnectionString,
                    new ModelBinder(
                        new ObjectBinder(new SimpleTypeBuilderAdapter(new ValueTypeBinder())),
                        new ObjectBinder(
                            new ClassTypeBinder(new ConcreteClassRowToObject(),
                                new AnonymousClassRowToObject())))), new BaseExpressionVisitor());
    }
}