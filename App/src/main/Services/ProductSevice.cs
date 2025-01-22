using Webshop.Models.DB;
using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;

using System;
using System.Drawing; // Namespace für Image
using System.IO;
using Webshop.Controllers;

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
        Product product = await _context.products.FirstOrDefaultAsync(p => p.ProductId == id);
        setImageWithAndHeight(product);
        
        return product;
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

    public void getRatingDetailsForTheProdct(int productProductId, Product product)
    {
        var rating = _context.productRatings.Where(r => r.ProductId == productProductId).FirstOrDefault();

        if (rating != null) 
        {
            product.TotalReviews = rating.TotalReviews;
            product.AverageRating = rating.AverageRating;
            product.OneStar = rating.OneStar;
            product.TwoStars = rating.TwoStars;
            product.ThreeStars = rating.ThreeStars;
            product.FourStars = rating.FourStars;
            product.FiveStars = rating.FiveStars;
        }
        else
        {
            product.TotalReviews = 0;
            product.AverageRating = 0;
            product.OneStar = 0;
            product.TwoStars = 0;
            product.ThreeStars = 0;
            product.FourStars = 0;
            product.FiveStars = 0;
        }

    }

    private void setImageWithAndHeight(Product product)
    {
        string apiPath = product.ProductImage;
        string filePath = apiPath.Replace("/api/assets/", "wwwroot/assets/");

        // Prüfen, ob die Datei existiert
        if (File.Exists(filePath))
        {
            // Bild laden und Maße auslesen
            using (var image = Image.FromFile(filePath))
            {
                product.ImageWidth = image.Width.ToString();
                product.ImageHeight = image.Height.ToString();
            }
        }
        else
        {
            Console.WriteLine("Datei nicht gefunden!");
        }
    }

    public void getStockForTheProduct(int productProductId, Product product)
    {
        var inventory = _context.inventory.Where(i => i.ProductId == productProductId).FirstOrDefault();
        product.Stock = inventory.Quantity;
    }

    public void getReviewsForTheProduct(int productProductId, ProductController.ProductAndReviewResponse productAndReviewResponse)
    {
        var reviews = _context.reviews.Where(r => r.ProductId == productProductId).ToList();

        foreach (var review in productAndReviewResponse.Reviews)
        {
            var user = _context.users.Where(u => u.CustomerId == review.CustomerId).FirstOrDefault();
            review.UserDisplayName = user.DisplayName;
            review.UserPictureDataUrl = user.PictureDataUrl;
        }
        
        productAndReviewResponse.Reviews = reviews;
    }
}