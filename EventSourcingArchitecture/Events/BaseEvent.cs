namespace OnlineOrderSystem.Events
{
    public abstract class BaseEvent
    {
        public Guid Id { get; protected set; }
        public Guid AggregateId { get; protected set; }
        public DateTime OccurredOn { get; protected set; }
        public int Version { get; protected set; }
        public string EventType { get; protected set; }

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