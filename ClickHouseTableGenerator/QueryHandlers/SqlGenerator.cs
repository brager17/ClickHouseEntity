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
        private readonly IQuery<CreateDbInfo, HasSqlStringInfo> _getSqlString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineDbInfoFactory"> Фабрика определяющая как строить EngineDbInfo, либо по атрибутам в классе,
        /// либо по DbSetSettings</param>
        /// <param name="cSharpToDbType">Интерфейс маппинга типов из c# в типы ClickHouse</param>
        /// <param name="getSqlString">Строить SQL по CreateDbInfo</param>
        public SqlGenerator(
            IQueryFactory<ClassType, IQuery<ClassType, EngineDbInfo>> engineDbInfoFactory,
            IQuery<Type, CreatingDbType> cSharpToDbType,
            IQuery<CreateDbInfo, HasSqlStringInfo> getSqlString)
        {
            _engineDbInfoFactory = engineDbInfoFactory;
            _cSharpToDbType = cSharpToDbType;
            _getSqlString = getSqlString;
        }

        public HasSqlStringInfo Query(ClassType input)
        {
            var props = input.classType.GetProperties().ToList();
            var creatingDBType = new CreateDbInfo
            {
                TableName = input.classType.GetNameAttributeValueEnumMember<string>(),
                DbColumns = props.Select(x => new DBColumn
                {
                    Name = x.GetColumnName(),
                    DbTypeName = _cSharpToDbType.Query(x.PropertyType).GetNameAttributeValueEnumMember()
                }).ToList(),
                EngineDbInfo = _engineDbInfoFactory.Create(input).Query(input),
            };
            return _getSqlString.Query(creatingDBType);
        }
    }
}