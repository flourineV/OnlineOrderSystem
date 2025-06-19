using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Events
{
    public class OrderUpdatedEvent : BaseEvent
    {
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }

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

        // Parameterless constructor for JSON deserialization
        public OrderUpdatedEvent() : base()
        {
            Items = new List<OrderItem>();
        }
    }
}