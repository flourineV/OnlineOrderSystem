namespace OnlineOrderSystem.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public Product? Product { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        // Calculated property
        public decimal Subtotal => Price * Quantity;

        public OrderItem(Guid productId, decimal price, int quantity)
        {
            // Validation
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot be empty");
            if (price <= 0)
                throw new ArgumentException("Price must be positive");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        // For serialization
        private OrderItem() { }
    }
}