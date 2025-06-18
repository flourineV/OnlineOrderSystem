namespace OnlineOrderSystem.Queries
{
    public class GetOrderByIdQuery
    {
        public Guid OrderId { get; }

        public GetOrderByIdQuery(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));

            OrderId = orderId;
        }
    }
}
