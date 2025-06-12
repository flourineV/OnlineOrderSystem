namespace OnlineOrderSystem.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        // Calculated property
        public decimal Subtotal => Price * Quantity;

        public OrderItem(Guid productId, string productName, decimal price, int quantity)
        {
            // Validation
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot be empty");
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("ProductName cannot be empty");
            if (price <= 0)
                throw new ArgumentException("Price must be positive");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        // For serialization
        private OrderItem() { }
    }
}