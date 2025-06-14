namespace OnlineOrderSystem.EventStore
{
    /// <summary>
    /// Model để lưu trữ events trong EventStore
    /// </summary>
    public class EventModel
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventData { get; set; } = string.Empty; // JSON serialized event
        public int Version { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
