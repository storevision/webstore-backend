using Webshop.Models.DB;
using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;

namespace Webshop.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductService(ApplicationDbContext context, IServiceScopeFactory serviceScopeFactory)
    {
        _context = context;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        List<Product> productList = new List<Product>();
        
        Product[] products = await _context.products.ToArrayAsync();
        Inventory[] inventorys = context.inventory.ToArray();
        
        foreach (Product product in products)
        {
            Inventory inventory = new Inventory();
            inventory = inventorys.FirstOrDefault(i => i.ProductId == product.ProductId);
            product.Stock = inventory.Quantity;
            
            
            
            productList.Add(product);
        }
        
        
        return productList;
    }

    // Methode: Produkt nach ID abrufen
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.products.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public Product CreateProduct(string name, string description, decimal price)
    {
        Product product = new Product();
        product.ProductName = name;
        product.ProductDescription = description;
        product.ProductPricePerUnit = price;
        AddProductAsync(product);
        return product;
    }

    // Methode: Neues Produkt hinzufügen
    public async Task AddProductAsync(Product product)
    {
        _context.products.Add(product);
        await _context.SaveChangesAsync();
    }

    // Methode: Produkt löschen
    /// <summary>
    ///     Asynchronously deletes a product from the database by its ID.
    /// </summary>
    /// <remarks>
    ///     Use <see langword="await" /> to ensure asynchronous operations are completed before calling other methods
    ///     on the same <see cref="DbContext" />. See 
    ///     <see href="https://aka.ms/efcore-docs-threading">EF Core threading issues</see> for details.
    /// </remarks>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteProductAsync(int id)
    {
        
        var product = await _context.products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product != null)
        {
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    // TODO needs to handle the given parameters to add a review referenced to the given product
    public async Task AddProductReviewAsync(int id, int rating, string review)
    {
        
    }
    
    // TODO needs to handle the given parameters to edit the review referenced to the given product
    public async Task EditProductReviewAsync(int id, int rating, string review)
    {
        
    }
    
    // TODO needs to handle the given parameters to delete a review referenced to the given product
    public async Task DeleteProductReviewAsync(int productId)
    {
        
    }
    
}