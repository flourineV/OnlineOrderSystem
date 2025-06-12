using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Commands
{
    public class UpdateOrderCommand
    {
        public Guid OrderId { get; }
        public List<OrderItem> Items { get; }

        public UpdateOrderCommand(Guid orderId, List<OrderItem> items)
        {
            OrderId = orderId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
    }
}