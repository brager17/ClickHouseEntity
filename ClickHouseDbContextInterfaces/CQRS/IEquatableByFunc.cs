namespace ClickHouseDbContextExntensions.CQRS
{
    /// <summary>
    /// Нужно писать максимально производительный 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEquatableByFunc<T>
    {
        bool Equals(T item1, T item2);
    }
}