namespace DbContext
{
    public interface IOperationRequestHandle<T>
    {
        string Handle(T operationInfo);
    }
}