namespace Shared.Domain
{
    public interface IApplicationEvent : IDomainEvent
    {
        string EventType { get; }
    }
}