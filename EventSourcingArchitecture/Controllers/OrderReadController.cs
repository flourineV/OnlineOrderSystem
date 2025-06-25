using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineOrderSystem.DTO;
using OnlineOrderSystem.Queries;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.ReadModel;

namespace OnlineOrderSystem.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderReadController : ControllerBase
    {
        private readonly GetOrderByIdHandler _getOrderByIdHandler;
        private readonly IOrderReadModelRepository _readModelRepository;
        private readonly ILogger<OrderReadController> _logger;
        private readonly IProductRepository _productRepository;

        public OrderReadController(
            GetOrderByIdHandler getOrderByIdHandler,
            IOrderReadModelRepository readModelRepository,
            ILogger<OrderReadController> logger,
            IProductRepository productRepository)
        {
            _getOrderByIdHandler = getOrderByIdHandler;
            _readModelRepository = readModelRepository;
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("Retrieving all orders");

                var readModels = await _readModelRepository.GetAllAsync();

                var orders = readModels.Select(rm => new OrderResponse
                {
                    Id = rm.Id,
                    CustomerId = rm.CustomerId,
                    Items = rm.Items.Select(item => new OrderItemResponse
                    {
                        ProductId = item.ProductId,
                        Product = _productRepository.GetById(item.ProductId),
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Subtotal
                    }).ToList(),
                    Status = rm.Status,
                    CreatedAt = rm.CreatedAt,
                    UpdatedAt = rm.UpdatedAt,
                    TotalAmount = rm.TotalAmount,
                    ShippingAddress = rm.ShippingAddress
                }).ToList();

                _logger.LogInformation("Retrieved {OrderCount} orders", orders.Count);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving all orders: {Error}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving order with ID {OrderId}", orderId);
                var query = new GetOrderByIdQuery(orderId);
                var orderResponse = await _getOrderByIdHandler.Handle(query);
                return Ok(orderResponse);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Error}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the order: {Error}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
