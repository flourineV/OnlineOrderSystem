using OnlineOrderSystem.Events;
using System.Collections.Concurrent;

namespace OnlineOrderSystem.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<BaseEvent, Task>>> _handlers = new();
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        public async Task PublishAsync<T>(T @event) where T : BaseEvent
        {
            _logger.LogInformation("Publishing event {EventType} for aggregate {AggregateId}",
                @event.EventType, @event.AggregateId);

            var eventType = typeof(T);
            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                var tasks = handlers.Select(handler => handler(@event));
                await Task.WhenAll(tasks);

                _logger.LogInformation("Successfully published event {EventType} to {HandlerCount} handlers",
                    @event.EventType, handlers.Count);
            }
            else
            {
                _logger.LogInformation("No handlers registered for event type {EventType}", @event.EventType);
            }
        }

        public async Task PublishAsync(IEnumerable<BaseEvent> events)
        {
            _logger.LogInformation("Publishing {EventCount} events", events.Count());

            foreach (var @event in events)
            {
                var eventType = @event.GetType();
                if (_handlers.TryGetValue(eventType, out var handlers))
                {
                    var tasks = handlers.Select(handler => handler(@event));
                    await Task.WhenAll(tasks);
                }
            }

            _logger.LogInformation("Successfully published {EventCount} events", events.Count());
        }

        public void Subscribe<T>(Func<T, Task> handler) where T : BaseEvent
        {
            var eventType = typeof(T);
            var wrappedHandler = new Func<BaseEvent, Task>(@event => handler((T)@event));

            _handlers.AddOrUpdate(
                eventType,
                new List<Func<BaseEvent, Task>> { wrappedHandler },
                (key, existing) =>
                {
                    existing.Add(wrappedHandler);
                    return existing;
                });

            _logger.LogInformation("Subscribed handler for event type {EventType}", eventType.Name);
        }

        public void Subscribe(Type eventType, Func<BaseEvent, Task> handler)
        {
            _handlers.AddOrUpdate(
                eventType,
                new List<Func<BaseEvent, Task>> { handler },
                (key, existing) =>
                {
                    existing.Add(handler);
                    return existing;
                });

            _logger.LogInformation("Subscribed handler for event type {EventType}", eventType.Name);
        }
    }
}
