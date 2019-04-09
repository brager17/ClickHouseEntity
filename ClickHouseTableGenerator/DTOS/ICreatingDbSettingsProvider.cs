namespace ClickHouseTableGenerator
{
    /// <summary>
    /// Реализуйте этот интерфейс,и укажите как аргумент атрибута DbSetSettings
    /// </summary>
    public interface ICreatingDbSettingsProvider
    {
        EngineDbInfo Create();
    }
}