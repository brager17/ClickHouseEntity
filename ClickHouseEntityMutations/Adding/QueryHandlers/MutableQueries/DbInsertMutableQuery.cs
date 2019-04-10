using ClickHouse.Ado;
using ClickHouseDbContextExntensions.CQRS;

namespace EntityTracking
{
    public class DbInsertMutableQuery : IMutableQuery<DbCommandMutableInfo<AddingSql>, ClickHouseCommand>
    {
        public ClickHouseCommand Query(DbCommandMutableInfo<AddingSql> input)
        {
            var command = input.Command;
            command.Parameters.Add(input.Info.ClickHouseParameter);
            return command;
        }
    }
}