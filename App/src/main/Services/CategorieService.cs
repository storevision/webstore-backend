using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;
using Webshop.Models.DB;

namespace Webshop.Services;

public class CategorieService
{
    private readonly ApplicationDbContext _context;

    public CategorieService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.categories.ToListAsync();
    }
    
}