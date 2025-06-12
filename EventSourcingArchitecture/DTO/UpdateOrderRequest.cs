namespace OnlineOrderSystem.DTO
{
    public class UpdateOrderRequest
    {
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}