using Microsoft.AspNetCore.Mvc;
using Webshop.Services;
using Webshop.Models.Products;

namespace Webshop.Controllers;

[ApiController]
[Route("Products")]
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
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}