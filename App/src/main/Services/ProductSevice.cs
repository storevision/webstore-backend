using Webshop.Models.DB;
using Microsoft.EntityFrameworkCore;
using Webshop.Models.Products;

namespace Webshop.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Methode: Alle Produkte abrufen
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.products.ToListAsync();
    }

    // Methode: Produkt nach ID abrufen
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.products.FirstOrDefaultAsync(p => p.productid == id);
    }

    // Methode: Neues Produkt hinzufügen
    public async Task AddProductAsync(Product product)
    {
        _context.products.Add(product);
        await _context.SaveChangesAsync();
    }

    // Methode: Produkt löschen
    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.products.FirstOrDefaultAsync(p => p.productid == id);
        if (product != null)
        {
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}