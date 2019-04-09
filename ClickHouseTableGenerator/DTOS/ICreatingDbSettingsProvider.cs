namespace ClickHouseTableGenerator
{
    /// <summary>
    /// Реализуйте этот интерфейс,когда используете SQL функции при опеделении партиции/ключа сортировки/сэмплирования
    /// и укажите как аргумент атрибута DbSetSettings
    /// </summary>
    public interface ICreatingDbSettingsProvider
    {
        EngineDbInfo Create();
    }
}