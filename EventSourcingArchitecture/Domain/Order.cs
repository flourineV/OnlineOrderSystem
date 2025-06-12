using OnlineOrderSystem.Events;

namespace OnlineOrderSystem.Domain
{
    public class Order
    {
        // Identity
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }

        // Value objects
        private readonly List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        // Properties
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string ShippingAddress { get; private set; }
        public int Version { get; private set; }

        // Calculated property
        public decimal TotalAmount => _items.Sum(item => item.Subtotal);

        // Event sourcing
        private readonly List<BaseEvent> _uncommittedEvents = new();

        // Constructor cho PlaceOrderCommand
        public Order(Guid customerId, List<OrderItem> items, string shippingAddress)
        {
            // Business validation
            if (customerId == Guid.Empty)
                throw new ArgumentException("CustomerId cannot be empty");
            if (items == null || !items.Any())
                throw new ArgumentException("Order must have at least one item");
            if (string.IsNullOrWhiteSpace(shippingAddress))
                throw new ArgumentException("ShippingAddress cannot be empty");

            // Initialize
            Id = Guid.NewGuid();
            CustomerId = customerId;
            _items.AddRange(items);
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            ShippingAddress = shippingAddress;
            Version = 0;

            // Raise domain event
            AddEvent(new OrderPlacedEvent(Id, CustomerId, Items.ToList(), TotalAmount, ShippingAddress, Version + 1));
        }

        // Private constructor for Event Sourcing reconstruction
        private Order() { }

        // Business method cho UpdateOrderCommand
        public void UpdateItems(List<OrderItem> newItems)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Can only update items for pending orders");

            if (newItems == null || !newItems.Any())
                throw new ArgumentException("Order must have at least one item");

            _items.Clear();
            _items.AddRange(newItems);
            UpdatedAt = DateTime.UtcNow;

            AddEvent(new OrderUpdatedEvent(Id, Items.ToList(), TotalAmount, Version + 1));
        }

        // Business method cho CancelOrderCommand
        public void Cancel(string reason = "")
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled");

            Status = OrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
            AddEvent(new OrderCancelledEvent(Id, reason, Version + 1));
        }

        // Event sourcing methods
        private void AddEvent(BaseEvent @event)
        {
            _uncommittedEvents.Add(@event);
            Version++;
        }

        public IEnumerable<BaseEvent> GetUncommittedEvents() => _uncommittedEvents.AsReadOnly();

        public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

        // Reconstruction từ events (cho UpdateOrderCommand và CancelOrderCommand)
        public static Order FromEvents(IEnumerable<BaseEvent> events)
        {
            var order = new Order();

            foreach (var @event in events.OrderBy(e => e.Version))
            {
                order.Apply(@event);
            }

            return order;
        }

        private void Apply(BaseEvent @event)
        {
            switch (@event)
            {
                case OrderPlacedEvent e:
                    Id = e.AggregateId;
                    CustomerId = e.CustomerId;
                    _items.AddRange(e.Items);
                    Status = OrderStatus.Pending;
                    CreatedAt = e.OccurredOn;
                    ShippingAddress = e.ShippingAddress;
                    Version = e.Version;
                    break;

                case OrderUpdatedEvent e:
                    _items.Clear();
                    _items.AddRange(e.Items);
                    UpdatedAt = e.OccurredOn;
                    Version = e.Version;
                    break;

                case OrderCancelledEvent e:
                    Status = OrderStatus.Cancelled;
                    UpdatedAt = e.OccurredOn;
                    Version = e.Version;
                    break;
            }
        }
    }
}