using Microsoft.AspNetCore.Http.HttpResults;
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

    
    // Response a list of all existing products
    /** Product has to be modified 
     *
    {
  "success": true,
  "data": [
    {
      "id": 0,
      "name": "string",
      "price_per_unit": 0,
      "description": "string",
      "category_id": 0,
      "image_url": "string",
      "image_width": 0,
      "image_height": 0,
      "blurred_image": "string",
      "blurred_image_width": 0,
      "blurred_image_height": 0,
      "stock": 0,
      "avg_rating": 0
    }
  ]
} *
     */
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

    // Get the selected product based on its id
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
    [Route("review/add")]
    public async Task<IActionResult> AddReview([FromBody] ReviewRequestBody review)
    {
        _productService.AddProductReviewAsync(review.product_id, review.rating, review.comment);
        return Ok();
    }
    
    /**
    the response should return the status of the operation
    success: true or false
     */
    
    [HttpPost]
    [Route("review/edit")]
    public async Task<IActionResult> EditReview([FromBody] ReviewRequestBody review)
    {
        _productService.EditProductReviewAsync(review.product_id, review.rating, review.comment);
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
    
    public class ReviewRequestBody
    {
        public int product_id { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }
    }
    
}