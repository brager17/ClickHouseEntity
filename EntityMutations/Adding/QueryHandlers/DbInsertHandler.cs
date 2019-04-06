using System.Collections.Generic;
using System.Linq;
using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;
using ExpressionTreeVisitor;

namespace EntityTracking
{
    public class DbInsertHandler : IHandler<AddingSql>
    {
        private readonly string _connectionString;

        public DbInsertHandler(string connectionString)
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
}