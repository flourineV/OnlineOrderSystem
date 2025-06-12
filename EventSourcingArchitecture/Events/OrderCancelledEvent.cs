namespace OnlineOrderSystem.Events
{
    public class OrderCancelledEvent : BaseEvent
    {
        public string Reason { get; private set; }

        // Constructor cho khi tạo event mới
        public OrderCancelledEvent(
            Guid aggregateId,
            string reason,
            int version)
            : base(aggregateId, version)
        {
            Reason = reason ?? string.Empty;
        }

        // For deserialization
        private OrderCancelledEvent() : base() { }
    }
}