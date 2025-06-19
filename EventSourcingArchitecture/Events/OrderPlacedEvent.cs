using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Events
{
    public class OrderPlacedEvent : BaseEvent
    {
        public Guid CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;

        // Constructor cho khi tạo event mới
        public OrderPlacedEvent(
            Guid aggregateId,
            Guid customerId,
            List<OrderItem> items,
            decimal totalAmount,
            string shippingAddress,
            int version)
            : base(aggregateId, version)
        {
            CustomerId = customerId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        }

        // Parameterless constructor for JSON deserialization
        public OrderPlacedEvent() : base()
        {
            Items = new List<OrderItem>();
            ShippingAddress = string.Empty;
        }
    }
}