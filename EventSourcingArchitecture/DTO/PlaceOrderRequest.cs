namespace OnlineOrderSystem.DTO
{
    public class PlaceOrderRequest
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
        public string ShippingAddress { get; set; } = string.Empty;
    }

    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}