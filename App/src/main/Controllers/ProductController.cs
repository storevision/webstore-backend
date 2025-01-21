using System.Net;
using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models.ApiHelper;
using Webshop.App.src.main.Models.Responses;
using Webshop.App.src.main.Models;
using Webshop.Services;

namespace Webshop.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ApiHelper
{
  
    // Inject the ProductService
    private readonly ProductService _productService;

    // When the ProductController is created, the ProductService is injected
    public ProductController(ProductService productService)
    {
        _productService = productService;
    }
    
    // Get all products
    [HttpGet("list")]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            
            return this.SendSuccess(products);

        }
        catch (Exception e)
        {
            return this.SendError(HttpStatusCode.InternalServerError, "An error occurred while fetching the products.");
        }
    }

    // Get the selected product based on its id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return this.SendSuccess(product);
    }

    // Unused
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] string name, [FromForm] string description, [FromForm] decimal price)
    {
        Product product = _productService.CreateProduct(name, description, price);
        return Ok(product);
    }

    // Unused
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        
        return NoContent();
    }

    /**
    the response should return the status of the operation
    success: true or false
    dont forget to handle exceptions
     */

    [HttpPost]
    [Route("productReview/add")]
    public async Task<IActionResult> AddReview([FromBody] ProductReviewRequestBody productReview)
    {
        _productService.AddProductReviewAsync(productReview.product_id, productReview.rating, productReview.comment);
        return Ok();
    }
    
    /**
    the response should return the status of the operation
    success: true or false
     */
    
    [HttpPost]
    [Route("productReview/edit")]
    public async Task<IActionResult> EditReview([FromBody] ProductReviewRequestBody productReview)
    {
        _productService.EditProductReviewAsync(productReview.product_id, productReview.rating, productReview.comment);
        return Ok();
    }
    
    /**
    the response should return the status of the operation
    success: true or false
     */
    
    [HttpPost]
    [Route("review/delete")]
    public async Task<IActionResult> DeleteReview(int product_id)
    {
        _productService.DeleteProductReviewAsync(product_id);
        return Ok();
    }
    
    public class ProductReviewRequestBody
    {
        public int product_id { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }
    }
    
}