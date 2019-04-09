using System.Diagnostics;
using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseDbContextExntensions.DTOS;

namespace EntityTracking
{
    public class WriteDbHandler<T> : IHandler<T> where T : HasSqlStringInfo
    {
        private readonly string _connectionString;
        private readonly IMutableQuery<DbCommandMutableInfo<T>, ClickHouseCommand> _commandQuery;

        public WriteDbHandler(
            string connectionString,
            IMutableQuery<DbCommandMutableInfo<T>, ClickHouseCommand> commandQuery)
        {
            _connectionString = connectionString;
            _commandQuery = commandQuery;
        }

        public void Handle(T input)
        {
            var settings = new ClickHouseConnectionSettings(_connectionString);
            using (var cnn = new ClickHouseConnection(settings))
            {
                cnn.Open();
                var command = cnn.CreateCommand(input.Sql);
                command = _commandQuery.Query(new DbCommandMutableInfo<T> {Info = input, Command = command});
                command.ExecuteNonQuery();
                cnn.Close();
            }
        }
    }
}