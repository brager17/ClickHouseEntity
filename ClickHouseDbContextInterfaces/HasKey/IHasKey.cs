namespace ExpressionTreeVisitor
{
    public interface IHasKey
    {
        object ObjectKey { get; set; }
    }

    public interface IHasKey<T> : IHasKey
    {
        T Key { get; set; }
    }
}