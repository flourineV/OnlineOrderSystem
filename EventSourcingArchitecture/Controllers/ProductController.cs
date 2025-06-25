using Microsoft.AspNetCore.Mvc;

namespace OnlineOrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(
    ILogger<ProductController> _logger,
    IProductRepository _productRepository
) : ControllerBase
{
    [HttpGet("api/products")]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        _logger.LogInformation("Retrieving all products");
        try
        {
            var products = _productRepository.GetAll();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return StatusCode(500, "Internal server error");
        }
    }
}