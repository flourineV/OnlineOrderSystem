using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.ReadModel
{
    public class OrderReadModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderItemReadModel> Items { get; set; } = new();
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public int Version { get; set; }
    }

    public class OrderItemReadModel
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
