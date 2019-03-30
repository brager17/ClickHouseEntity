using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouse.Ado;

namespace DbContext
{
    public class DbHandler : IDbHandler
    {
        private readonly string _connectionString;
        private readonly IModelBinder _modelBinder;

        public DbHandler(string connectionString, IModelBinder modelBinder)
        {
            _connectionString = connectionString;
            _modelBinder = modelBinder;
        }

        public T GetData<T>(string sqlCommand, BindInfo bindInfo)
        {
            using (var cnn = new ClickHouseConnection(new ClickHouseConnectionSettings(_connectionString)))
            {
                cnn.Open();
                var reader = cnn.CreateCommand(sqlCommand).ExecuteReader();
                var result = _modelBinder.Bind<T>(reader, bindInfo);
                cnn.Close();
                return result;
            }
        }
    }
}