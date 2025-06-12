namespace OnlineOrderSystem.Commands
{
    public class CancelOrderCommand
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public CancelOrderCommand(Guid orderId, string reason = "")
        {
            OrderId = orderId;
            Reason = reason ?? string.Empty;
        }
    }
}