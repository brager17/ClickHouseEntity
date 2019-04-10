namespace ClickHouseDbContextExntensions.CQRS
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }
}