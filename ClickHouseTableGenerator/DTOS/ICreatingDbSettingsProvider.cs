namespace ClickHouseTableGenerator
{
    public interface ICreatingDbSettingsProvider
    {
        EngineDbInfo Create();
    }
}