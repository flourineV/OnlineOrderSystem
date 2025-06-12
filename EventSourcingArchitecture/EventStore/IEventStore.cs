using OnlineOrderSystem.Events;

namespace OnlineOrderSystem.EventStore
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId, int fromVersion);
        Task<IEnumerable<BaseEvent>> GetAllEventsAsync();
    }
}
