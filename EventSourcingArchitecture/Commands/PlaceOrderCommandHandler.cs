using OnlineOrderSystem.Domain;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.EventBus;

namespace OnlineOrderSystem.Commands
{
    public class PlaceOrderCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PlaceOrderCommandHandler> _logger;

        public PlaceOrderCommandHandler(
            IEventStore eventStore,
            IEventBus eventBus,
            ILogger<PlaceOrderCommandHandler> logger)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<Guid> Handle(PlaceOrderCommand command)
        {
            _logger.LogInformation("Handling PlaceOrderCommand for customer {CustomerId}", command.CustomerId);

            // Tạo domain object - Business logic
            var order = new Order(command.CustomerId, command.Items, command.ShippingAddress);

            // Lấy events từ domain
            var events = order.GetUncommittedEvents();

            // Lưu events vào EventStore
            await _eventStore.SaveEventsAsync(order.Id, events, 0);

            // Publish events qua EventBus
            await _eventBus.PublishAsync(events);

            // Clear events
            order.ClearUncommittedEvents();

            _logger.LogInformation("Order {OrderId} placed successfully", order.Id);

            return order.Id;
        }
    }
}