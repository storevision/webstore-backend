using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.Services;

namespace Webshop.Controllers;

[ApiController]
[Route("categories")]
public class CategorieController : ControllerBase
{
    
    private readonly CategorieService _categorieService;

    public CategorieController(CategorieService categorieService)
    {
        _categorieService = categorieService;
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCategories()
    {
        List<Category> categories = await _categorieService.GetAllCategoriesAsync();
        CreatedResponse<Category> categoriesResponse = new CreatedResponse<Category>()
        {
            success = false
        };
        categoriesResponse.createSuccessListResponse(true, categories);
        return Ok(categoriesResponse);
    }
}
