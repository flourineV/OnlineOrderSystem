namespace OnlineOrderSystem.Events
{
    public class OrderCancelledEvent : BaseEvent
    {
        public string Reason { get; set; } = string.Empty;

        // Constructor cho khi tạo event mới
        public OrderCancelledEvent(
            Guid aggregateId,
            string reason,
            int version)
            : base(aggregateId, version)
        {
            Reason = reason ?? string.Empty;
        }

        // Parameterless constructor for JSON deserialization
        public OrderCancelledEvent() : base()
        {
            Reason = string.Empty;
        }
    }
}