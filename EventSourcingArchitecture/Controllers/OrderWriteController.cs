using Microsoft.AspNetCore.Mvc;
using OnlineOrderSystem.Commands;
using OnlineOrderSystem.DTO;
using OnlineOrderSystem.Domain; // Thêm using này

namespace OnlineOrderSystem.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderWriteController : ControllerBase
    {
        private readonly PlaceOrderCommandHandler _placeOrderHandler;
        private readonly UpdateOrderCommandHandler _updateOrderHandler;
        private readonly CancelOrderCommandHandler _cancelOrderHandler;
        private readonly ILogger<OrderWriteController> _logger;

        public OrderWriteController(
            PlaceOrderCommandHandler placeOrderHandler,
            UpdateOrderCommandHandler updateOrderHandler, // ✅ Sửa tên parameter
            CancelOrderCommandHandler cancelOrderHandler,
            ILogger<OrderWriteController> logger)
        {
            _placeOrderHandler = placeOrderHandler;
            _updateOrderHandler = updateOrderHandler; // ✅ Sửa assignment
            _cancelOrderHandler = cancelOrderHandler;
            _logger = logger;
        }

        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Placing new order for customer {CustomerId}", request.CustomerId);

                // Convert DTO → Command
                var command = new PlaceOrderCommand(
                    request.CustomerId,
                    request.Items.Select(item => new OrderItem( // ✅ Bỏ Domain. prefix
                        item.ProductId,
                        item.ProductName,
                        item.Price,
                        item.Quantity
                    )).ToList(),
                    request.ShippingAddress
                );

                // Gửi command đến handler
                var orderId = await _placeOrderHandler.Handle(command);

                _logger.LogInformation("Order {OrderId} placed successfully", orderId);

                return Created($"/api/orders/{orderId}", orderId);             
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid request for placing order: {Error}", ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing order");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Cập nhật đơn hàng (chỉ khi status = Pending)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Updating order {OrderId}", id);

                // Convert DTO → Command
                var command = new UpdateOrderCommand(
                    id,
                    request.Items.Select(item => new OrderItem(
                        item.ProductId,
                        item.ProductName,
                        item.Price,
                        item.Quantity
                    )).ToList()
                );

                // Gửi command đến handler
                await _updateOrderHandler.Handle(command);

                _logger.LogInformation("Order {OrderId} updated successfully", id);

                return NoContent(); // 204 - Update thành công
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid request for updating order {OrderId}: {Error}", id, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Cannot update order {OrderId}: {Error}", id, ex.Message);
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId}", id);
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest? request = null)
        {
            try
            {
                _logger.LogInformation("Cancelling order {OrderId}", id);

                // Convert DTO → Command
                var command = new CancelOrderCommand(id, request?.Reason ?? "Customer requested cancellation");

                // Gửi command đến handler
                await _cancelOrderHandler.Handle(command);

                _logger.LogInformation("Order {OrderId} cancelled successfully", id);

                return NoContent(); // 204 - Cancel thành công
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Cannot cancel order {OrderId}: {Error}", id, ex.Message);
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", id);
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }
    } // ✅ Đóng class đúng chỗ
}