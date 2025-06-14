using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineOrderSystem.DTO;
using OnlineOrderSystem.Queries;

namespace OnlineOrderSystem.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderReadController : ControllerBase
    {
        private readonly GetOrderByIdHandler _getOrderByIdHandler;
        private readonly ILogger<OrderReadController> _logger;

        public OrderReadController(
            GetOrderByIdHandler getOrderByIdHandler,
            ILogger<OrderReadController> logger)
        {
            _getOrderByIdHandler = getOrderByIdHandler;
            _logger = logger;
        }

        [HttpGet]
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

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdWithRoute(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving order with ID {OrderId} via route", orderId);
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
