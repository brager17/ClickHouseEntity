using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ClickDbContextInfrastructure;
using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;
using DbContext;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    public class DbSetOperations<T> : IDbSetOperations<T>
    {
        private readonly IHandler<IEnumerable<T>> _addEnumerableHandler;

        public DbSetOperations(IHandler<IEnumerable<T>> addEnumerableHandler)
        {
            _addEnumerableHandler = addEnumerableHandler;
        }

        public void Add(IEnumerable<T> items)
        {
            _addEnumerableHandler.Handle(items);
        }

        public void Remove(IEnumerable<T> item, IEnumerator<T> enumerator)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<T, bool>> exprFilter)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }

    [Info("Optimize code")]
    public class InsertInfo
    {
        public string TableName { get; set; }
        public string[] InsertColumns { get; set; }
        public object[][] Values { get; set; }
    }

    [Info("Optimize code")]
    public class InsertHandler<T> : IHandler<IEnumerable<T>>
    {
        private readonly IQuery<T[], InsertInfo> _insertInfoQuery;
        private readonly IQuery<InsertInfo, AddingSql> _insertToSqlQuery;
        private readonly IHandler<AddingSql> _insertHandler;

        public InsertHandler(
            IQuery<T[], InsertInfo> insertInfoQuery,
            IQuery<InsertInfo, AddingSql> insertToSqlQuery,
            IHandler<AddingSql> insertHandler)
        {
            _insertInfoQuery = insertInfoQuery;
            _insertToSqlQuery = insertToSqlQuery;
            _insertHandler = insertHandler;
        }

        public void Handle(IEnumerable<T> input)
        {
            _insertHandler.Handle(_insertToSqlQuery.Query(_insertInfoQuery.Query(input.ToArray())));
        }
    }

    [Info("Optimize code")]
    public class InsertInfoQuery<T> : IQuery<T[], InsertInfo>
    {
        public InsertInfo Query(T[] input)
        {
            var properties = typeof(T).GetProperties().ToArray();
            var tableName = typeof(T).GetCustomAttribute<TableNameAttribute>().Name;
            var insertPropertiesName = new string[properties.Length];
            var countRows = input.Length;
            var countProps = properties.Length;
            var values = new object[countRows][];

            for (int i = 0; i < countProps; i++)
            {
                insertPropertiesName[i] = properties[i].GetColumnName();
            }

            for (int i = 0; i < countRows; i++)
            {
                values[i] = new object[countProps];
                for (int j = 0; j < countProps; j++)
                {
                    values[i][j] = properties[j].GetValue(input[i]);
                }
            }

            return new InsertInfo
            {
                Values = values, TableName = tableName, InsertColumns = insertPropertiesName
            };
        }
    }

    public class AddingSql
    {
        public string Sql { get; set; }
        public ClickHouseParameter ClickHouseParameter { get; set; }
    }

    [Info("Optimize code")]
    public class InsertInfoToSqlString : IQuery<InsertInfo, AddingSql>
    {
        public AddingSql Query(InsertInfo input)
        {
            var valuesStr = "";
            for (int i = 0; i < input.Values.Length; i++)
            {
                valuesStr += $"({string.Join(",", input.Values[i])}),\n";
            }

            var result = new AddingSql
            {
                Sql = $"INSERT INTO {input.TableName} ({string.Join(',', input.InsertColumns)}) VALUES @bulk",
                ClickHouseParameter = new ClickHouseParameter
                {
                    DbType = DbType.Object,
                    ParameterName = "bulk",
                    Value = input.Values
                }
            };
            return result;
        }
    }

    public class InsertHandler : IHandler<AddingSql>
    {
        private readonly string _connectionString;

        public InsertHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Handle(AddingSql input)
        {
            var settings = new ClickHouseConnectionSettings(_connectionString);
            using (var cnn = new ClickHouseConnection(settings))
            {
                cnn.Open();
                var command = cnn.CreateCommand(input.Sql);
                command.Parameters.Add(input.ClickHouseParameter);
                command.ExecuteNonQuery();
                cnn.Close();
            }
        }
    }

    public class StringTransform : IQuery<object, string>
    {
        public string Query(object input) => $"'{input}'";
    }

    public class DefaultTransform : IQuery<object, string>
    {
        public string Query(object input) => $"{input}";
    }

    [Info("Optimize code")]
    public class DateTimeTransform : IQuery<object, string>
    {
        //input - DateTime
        public string Query(object input) => $"'{((dynamic) input).ToString("yyyy-MM-dd HH:mm:ss")}'";
    }

    public class ConditionQueryFactoryAdapter : IQuery<Type, IQuery<object, string>>
    {
        private readonly IQuery<object, string> _query;

        public ConditionQueryFactoryAdapter(IQuery<object, string> query)
        {
            _query = query;
        }

        public IQuery<object, string> Query(Type input) => _query;
    }
}