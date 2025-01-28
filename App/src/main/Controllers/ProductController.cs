using System.Net;
using System.Text.Json.Serialization;
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
    private readonly AuthService _authService;

    // When the ProductController is created, the ProductService is injected
    public ProductController(ProductService productService, AuthService authService)
    {
        _productService = productService;
        _authService = authService;
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
        catch (Exception)
        {
            return this.SendError(HttpStatusCode.InternalServerError, "An error occurred while fetching the products.");
        }
    }

    // Get the selected product based on its id
    [HttpGet("get")]
    public async Task<IActionResult> GetProductById([FromQuery] int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ProductAndReviewResponse productAndReviewResponse = new ProductAndReviewResponse
        {
            Product = null
        };
        _productService.getRatingDetailsForTheProdct(product.ProductId, product);
        _productService.getStockForTheProduct(product.ProductId, product);
        _productService.getReviewsForTheProduct(product.ProductId, productAndReviewResponse);
        productAndReviewResponse.Product = product;

        return this.SendSuccess(productAndReviewResponse);
    }

    // Unused
    [HttpPost]
    public Task<IActionResult> Add([FromForm] string name, [FromForm] string description, [FromForm] decimal price)
    {
        Task<Product> productTask = _productService.CreateProduct(name, description, price);
        var product = productTask.Result;
        return Task.FromResult<IActionResult>(Ok(product));
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
    [Route("review/add")]
    public async Task<IActionResult> AddReview([FromBody] ProductReviewRequestBody productReview)
    {
        await _productService.addProductReviewAsync(productReview.product_id, productReview.rating, productReview.comment, getUserId());
        return await Task.FromResult<IActionResult>(Ok(new { success = true }));
    }
    
    /**
    the response should return the status of the operation
    success: true or false
     */
    
    [HttpPost]
    [Route("review/edit")]
    public async Task<IActionResult> EditReview([FromBody] ProductReviewRequestBody productReview)
    {
        await _productService.updateProductReviewAsync(productReview.product_id, productReview.rating, productReview.comment, getUserId());
        return await Task.FromResult<IActionResult>(Ok(new { success = true }));
    }
    
    /**
    the response should return the status of the operation
    success: true or false
     */
    
    [HttpPost]
    [Route("review/delete")]
    public async Task<IActionResult> DeleteReview(int product_id)
    {
        await _productService.DeleteProductReviewAsync(product_id, getUserId());
        return await Task.FromResult<IActionResult>(Ok());
    }
    
    public class ProductReviewRequestBody
    {
        public int product_id { get; set; }
        public int rating { get; set; }
        public string? comment { get; set; }
    }
    
    public class ProductAndReviewResponse
    {
        [JsonPropertyName("product")]
        public required Product? Product { get; set; }
        [JsonPropertyName("reviews")]
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
    
    private int getUserId()
    {
        var token = Request.Cookies["token"];
        var userClaims = _authService.ValidateToken(token ?? throw new InvalidOperationException());
            
        try
        {
            if (userClaims == null)
            {
                throw new Exception("Token validation failed.");
            }
        }
        catch (Exception)
        {
            return -1;
        }
        return Convert.ToInt32(userClaims.FindFirst("id")?.Value);
    }
    
}