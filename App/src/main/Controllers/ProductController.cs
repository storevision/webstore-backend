using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.Services;
using Webshop.Models.Products;

namespace Webshop.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    /**
     * Hier wird eine Variable erzeugt in welcher der ProductService injekted wird
     */
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetProducts()
    {
        CreatedResponse<Product> createdResponse = new CreatedResponse<Product>
        {
            success = false
        };
        
        try
        {
            List<Product> products = await _productService.GetAllProductsAsync();
            
            createdResponse.createSuccessListResponse(true, products);
            return Ok(createdResponse);
            
        }
        catch (Exception e)
        {
            createdResponse.createErrorResponse(false, e.Message);
            return BadRequest(createdResponse);

        }
        
        
        
        
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] string name, [FromForm] string description, [FromForm] decimal price)
    {
        Product product = _productService.CreateProduct(name, description, price);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}