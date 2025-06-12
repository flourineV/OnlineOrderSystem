using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Commands
{
    public class PlaceOrderCommand
    {
        public Guid CustomerId { get; }
        public List<OrderItem> Items { get; }
        public string ShippingAddress { get; }

        public PlaceOrderCommand(Guid customerId, List<OrderItem> items, string shippingAddress)
        {
            CustomerId = customerId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        }
    }
}