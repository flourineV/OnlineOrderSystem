using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Events
{
    public class OrderUpdatedEvent : BaseEvent
    {
        public List<OrderItem> Items { get; private set; }
        public decimal TotalAmount { get; private set; }

        // Constructor cho khi tạo event mới
        public OrderUpdatedEvent(
            Guid aggregateId,
            List<OrderItem> items,
            decimal totalAmount,
            int version)
            : base(aggregateId, version)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalAmount = totalAmount;
        }

        // For deserialization
        private OrderUpdatedEvent() : base() { }
    }
}