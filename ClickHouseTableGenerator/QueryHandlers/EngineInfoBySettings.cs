using System.Reflection;
using ClickHouseDbContextExntensions.CQRS;
using ClickHouseTableGenerator;

namespace Root
{
    public class EngineInfoBySettings : IQuery<ClassType, EngineDbInfo>
    {
        public EngineDbInfo Query(ClassType input) => input.classType.GetCustomAttribute<DbSetSettings>().Key.Create();
    }
}