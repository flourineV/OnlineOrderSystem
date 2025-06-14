using OnlineOrderSystem.Events;
using System.Collections.Concurrent;

namespace OnlineOrderSystem.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, List<EventModel>> _events = new();
        private readonly ILogger<InMemoryEventStore> _logger;

        public InMemoryEventStore(ILogger<InMemoryEventStore> logger)
        {
            _logger = logger;
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            _logger.LogInformation("Saving {EventCount} events for aggregate {AggregateId}", events.Count(), aggregateId);

            var eventList = _events.GetOrAdd(aggregateId, _ => new List<EventModel>());

            lock (eventList)
            {
                // Version check for concurrency
                var currentVersion = eventList.Count;
                if (currentVersion != expectedVersion)
                {
                    throw new InvalidOperationException(
                        $"Concurrency conflict. Expected version {expectedVersion}, but current version is {currentVersion}");
                }

                // Add new events
                foreach (var @event in events)
                {
                    var eventModel = new EventModel
                    {
                        Id = @event.Id,
                        AggregateId = aggregateId,
                        EventType = @event.EventType,
                        EventData = System.Text.Json.JsonSerializer.Serialize(@event),
                        Version = @event.Version,
                        OccurredOn = @event.OccurredOn
                    };

                    eventList.Add(eventModel);
                }
            }

            _logger.LogInformation("Successfully saved {EventCount} events for aggregate {AggregateId}", events.Count(), aggregateId);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            _logger.LogInformation("Loading all events for aggregate {AggregateId}", aggregateId);

            if (!_events.TryGetValue(aggregateId, out var eventList))
            {
                _logger.LogWarning("No events found for aggregate {AggregateId}", aggregateId);
                return Enumerable.Empty<BaseEvent>();
            }

            var allEvents = eventList
                .OrderBy(e => e.Version)
                .Select(DeserializeEvent)
                .Where(e => e != null)
                .Cast<BaseEvent>()
                .ToList();

            _logger.LogInformation("Loaded {EventCount} events for aggregate {AggregateId}", allEvents.Count, aggregateId);
            return await Task.FromResult(allEvents);
        }

        public async Task<IEnumerable<BaseEvent>> GetAllEventsAsync()
        {
            _logger.LogInformation("Loading all events from store");

            var allEvents = _events.Values
                .SelectMany(eventList => eventList)
                .OrderBy(e => e.OccurredOn)
                .Select(DeserializeEvent)
                .Where(e => e != null)
                .Cast<BaseEvent>()
                .ToList();

            _logger.LogInformation("Loaded {EventCount} total events", allEvents.Count);
            return await Task.FromResult(allEvents);
        }

        private BaseEvent? DeserializeEvent(EventModel eventModel)
        {
            try
            {
                var eventType = Type.GetType($"OnlineOrderSystem.Events.{eventModel.EventType}");
                if (eventType == null)
                {
                    _logger.LogWarning("Unknown event type: {EventType}", eventModel.EventType);
                    return null;
                }

                var @event = System.Text.Json.JsonSerializer.Deserialize(eventModel.EventData, eventType) as BaseEvent;
                return @event;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize event {EventId} of type {EventType}",
                    eventModel.Id, eventModel.EventType);
                return null;
            }
        }
    }
}
