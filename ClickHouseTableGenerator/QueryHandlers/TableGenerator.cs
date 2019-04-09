using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using ClickDbContextInfrastructure;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;
using DbContext;
using EntityTracking;
using Root;

namespace ClickHouseTableGenerator
{
    public class TableGenerator : ITableGenerator
    {
        private readonly IQuery<ClassType, HasSqlStringInfo> _sqlGenerator;
        private readonly WriteDbHandler<HasSqlStringInfo> _writeDbHandler;

        public TableGenerator(
            IQuery<ClassType, HasSqlStringInfo> sqlGenerator,
            WriteDbHandler<HasSqlStringInfo> writeDbHandler)
        {
            _sqlGenerator = sqlGenerator;
            _writeDbHandler = writeDbHandler;
        }

        public void Generate(ClassType DbSetType)
        {
            DbSetType.classType.GetProperties()
                .Where(x => x.GetCustomAttribute<NoCreateTable>() == null).ToList()
                .Select(x => x.PropertyType)
                .Where(x => x.GetInterfaces().Any(xx => xx == typeof(IDbSet)))
                .Select(x => new ClassType {classType = x.GenericTypeArguments.Single()})
                .Select(_sqlGenerator.Query)
                .ToList()
                .ForEach(x => _writeDbHandler.Handle(x));
        }
    }
}