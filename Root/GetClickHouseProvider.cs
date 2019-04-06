using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using ClickHouseDbContextExntensions.CQRS;
using Context;
using DbContext;
using ExpressionTreeVisitor;

namespace Root
{
    public static class GetClickHouseProvider
    {
        #region helpers

        private static string GetMethodName(MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Method.Name;
        }

        private static UnaryExpression GetLambda(MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Arguments.Last() as UnaryExpression;
        }

        public static IEnumerable<Condition<LambdaListSelectInfo, AggregateLinqInfo>> AggregateLinqVisitorConditions()
        {
            return new List<Condition<LambdaListSelectInfo, AggregateLinqInfo>>
            {
                new Condition<LambdaListSelectInfo, AggregateLinqInfo>
                {
                    _predicate = x => GetMethodName(x.MethodCallExpression) == "Where"
                                      && x.MethodCallExpression.Arguments.Last() is UnaryExpression unary &&
                                      unary.Operand is LambdaExpression,
                    _selector = new WhereQuery(new WhereToSqlVisitor(new HasMapPropInfo()))
                },
                new Condition<LambdaListSelectInfo, AggregateLinqInfo>
                {
                    _predicate = x =>
                        new[] {"OrderBy", "OrderByDescending"}.Contains(GetMethodName(x.MethodCallExpression)) &&
                        GetLambda(x.MethodCallExpression) is UnaryExpression unaryExpression &&
                        unaryExpression.Operand is LambdaExpression,
                    _selector = new OrderQuery(new OrderingToSqlVisitor(new HasMapPropInfo()))
                },
                new Condition<LambdaListSelectInfo, AggregateLinqInfo>
                {
                    _predicate = x => x.MethodCallExpression.Method.Name == "Take",
                    _selector = new TakeQuery()
                },
                new Condition<LambdaListSelectInfo, AggregateLinqInfo>
                {
                    _predicate = x => true,
                    _selector = new DefaultQuery()
                }
            };
        }
        
        private static ClassTypeBinder GetClassTypeBinder()
        {
            return new ClassTypeBinder(
                new ConcreteClassNameValueListToObject(
                    new CacheQueryWithHashCodeDto<TypePropertiesInfo, Delegate>(
                        new Aggregate2Query<TypePropertiesInfo, LambdaExpression, Delegate>(
                            new BuildCreationInitializerLambda(),
                            new LambdaCompileQuery()))),
                new AnonymousClassNameValueListToObject());
        }


        private class TypePropertiesInfoComIEquatable : IEquatableByFunc<TypePropertiesInfo>
        {
            public bool Equals(TypePropertiesInfo item1, TypePropertiesInfo item2) =>
                item1.Type.Name == item2.Type.Name;
        }

        #endregion

        //todo add DI Container(хотя он тут нахуй не нужен
        public static ExpressionsToObject Get(string connectionString, IEnumerable<IDbLogger> loggers) =>
            new ExpressionsToObject(
                new LoggerDecaoratorToSqlConverter(
                    new ExpressionToSqlConverter(new SqlRequestHandler(
                            new SelectRequestHandler(),
                            new TableNameRequestHandler(),
                            new WhereOperationRequestHandler(),
                            new TakeOperationRequestHandle(),
                            new OrderOperationRequestHandle()),
                        new AggregateLinqVisitor(new ConditionQuery<LambdaListSelectInfo, AggregateLinqInfo>(
                            AggregateLinqVisitorConditions()))), loggers),
                new DbHandler(connectionString, new DataHandler(
                    new SimpleTypeObjectBinder(new ValueTypeBinder()),
                    new ComplexObjectBinder(GetClassTypeBinder()),
                    new ArrayObjectBinder(new SingleArrayObjectBinder()))),
                new VisitorsHandler(new PropertyMapInfoVisitor(new DtoToExpressionToLinqInfoHandler(),
                    new ValueTypeExpressionToLinqInfoHandler()), new AggregateLinqVisitor(
                    new ConditionQuery<LambdaListSelectInfo, AggregateLinqInfo>(
                        AggregateLinqVisitorConditions()))));

       
    }
}