using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;
using DbContext;
using ExpressionTreeVisitor;
using Root;

namespace ClickHouseTableGenerator
{
    public class SqlGenerator : IQuery<ClassType, HasSqlStringInfo>
    {
        private readonly IQueryFactory<ClassType, IQuery<ClassType, EngineDbInfo>> _engineDbInfoFactory;
        private readonly IQuery<Type, CreatingDbType> _cSharpToDbType;
        private readonly IQuery<CreatingDbInfo, HasSqlStringInfo> _getSqlString;

        public SqlGenerator(
            IQueryFactory<ClassType, IQuery<ClassType, EngineDbInfo>> engineDbInfoFactory,
            IQuery<Type, CreatingDbType> cSharpToDbType,
            IQuery<CreatingDbInfo, HasSqlStringInfo> getSqlString)
        {
            _engineDbInfoFactory = engineDbInfoFactory;
            _cSharpToDbType = cSharpToDbType;
            _getSqlString = getSqlString;
        }

        public HasSqlStringInfo Query(ClassType input)
        {
            var tableClassType = input.classType;
            var props = input.classType.GetProperties().ToList();
            var creatingDBType = new CreatingDbInfo
            {
                TableName = tableClassType.GetClassAttributeKey<string>(),
                DbColumns = props.Select(x => new DBColumn()
                {
                    Name = x.GetColumnName(),
                    DbTypeName = _cSharpToDbType.Query(x.PropertyType).GetClassAttributeKey()
                }).ToList(),
                EngineDbInfo = _engineDbInfoFactory.Create(input).Query(input),
            };
            return _getSqlString.Query(creatingDBType);
        }
    }
}