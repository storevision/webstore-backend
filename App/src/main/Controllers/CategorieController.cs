using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.ApiHelper;
using Webshop.Services;

namespace Webshop.Controllers;

// CategorieController is a controller class that handles requests for categories
[ApiController]
[Route("categories")]
public class CategorieController : ApiHelper
{
    
    private readonly CategorieService _categorieService;

    public CategorieController(CategorieService categorieService)
    {
        _categorieService = categorieService;
    }
    
    // Get all categories
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCategories()
    {
        List<Category> categories = await _categorieService.GetAllCategoriesAsync();
        return this.SendSuccess(categories);
    }
}
