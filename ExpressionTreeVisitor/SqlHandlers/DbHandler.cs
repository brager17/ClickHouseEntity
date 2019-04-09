using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ClickHouse.Ado;

namespace DbContext
{
    public class DbHandler : IDbHandler
    {
        private readonly string _connectionString;
        private readonly IDataHandler _dataHandler;

        public DbHandler(string connectionString, IDataHandler dataHandler)
        {
            _connectionString = connectionString;
            _dataHandler = dataHandler;
        }

        // todo рассмотреть вариант рефакторинга, считывать все в структуру данных и закрывать соединение,
        // todo а потом уже обрабатывать
        public T GetData<T>(string sqlCommand)
        {
            using (var cnn = new ClickHouseConnection(new ClickHouseConnectionSettings(_connectionString)))
            {
                cnn.Open();
                var reader = cnn.CreateCommand(sqlCommand).ExecuteReader();
                var result = _dataHandler.Handle<T>(reader);
                cnn.Close();
                return result;
            }
        }
    }
}