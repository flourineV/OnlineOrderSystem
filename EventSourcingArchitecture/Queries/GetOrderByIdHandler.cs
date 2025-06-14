using OnlineOrderSystem.DTO;
using OnlineOrderSystem.EventBus;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.ReadModel;

namespace OnlineOrderSystem.Queries
{
    public class GetOrderByIdHandler(
        IOrderReadModelRepository readModel,
        ILogger<GetOrderByIdHandler> logger)
    {
        private readonly ILogger<GetOrderByIdHandler> _logger = logger;
        private readonly IOrderReadModelRepository _readModel = readModel;

        public async Task<OrderResponse> Handle(GetOrderByIdQuery query)
        {
            _logger.LogInformation("Handling GetOrderByIdQuery for order {OrderId}", query.OrderId);
            var order = await _readModel.GetByIdAsync(query.OrderId);

            var orderResponse = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ShippingAddress = order.ShippingAddress,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };

            orderResponse.Items = [.. order.Items.Select(item => new OrderItemResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity,
                Subtotal = item.Subtotal
            })];

            _logger.LogInformation("Order {OrderId} retrieved successfully", query.OrderId);
            return orderResponse;
        }
    }
}
