using OnlineOrderSystem.Domain;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.EventBus;

namespace OnlineOrderSystem.Commands
{
    public class UpdateOrderCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(
            IEventStore eventStore,
            IEventBus eventBus,
            ILogger<UpdateOrderCommandHandler> logger)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(UpdateOrderCommand command)
        {
            _logger.LogInformation("Handling UpdateOrderCommand for order {OrderId}", command.OrderId);

            // Load order từ events (Event Sourcing)
            var events = await _eventStore.GetEventsAsync(command.OrderId);
            var order = Order.FromEvents(events);

            // Business logic - update items
            order.UpdateItems(command.Items);

            // Lấy new events
            var newEvents = order.GetUncommittedEvents();

            // Lưu events với version check
            await _eventStore.SaveEventsAsync(order.Id, newEvents, order.Version - newEvents.Count());

            // Publish events
            await _eventBus.PublishAsync(newEvents);

            order.ClearUncommittedEvents();

            _logger.LogInformation("Order {OrderId} updated successfully", command.OrderId);
        }
    }
}