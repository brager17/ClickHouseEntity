namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }

    public interface IQuery<in TIn, out TOut>
    {
        TOut Query(TIn input);
    }

  
}