namespace OnlineOrderSystem.Events
{
    public abstract class BaseEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public DateTime OccurredOn { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; } = string.Empty;

        protected BaseEvent(Guid aggregateId, int version)
        {
            Id = Guid.NewGuid();
            AggregateId = aggregateId;
            OccurredOn = DateTime.UtcNow;
            Version = version;
            EventType = GetType().Name;
        }

        // For deserialization/Event Sourcing reconstruction
        protected BaseEvent()
        {
            EventType = GetType().Name;
        }
    }
}