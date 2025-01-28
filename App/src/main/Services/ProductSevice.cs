using Webshop.Models.DB;
using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;
using SkiaSharp;

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
            inventory = inventorys.FirstOrDefault(i => i.ProductId == product.ProductId) ?? throw new InvalidOperationException();
            if (inventory.Quantity == null)
            {
                inventory.Quantity = 0;
            }
            product.Stock = (int)inventory.Quantity;
            
            productList.Add(product);
        }
        
        
        return productList;
    }

    // Methode: Produkt nach ID abrufen
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        Product product = await _context.products.FirstOrDefaultAsync(p => p.ProductId == id) ?? throw new InvalidOperationException("Product not found");
        setImageWithAndHeight(product);
        
        return product;
    }

    public async Task<Product> CreateProduct(string name, string description, decimal price)
    {
        Product product = new Product();
        product.ProductName = name;
        product.ProductDescription = description;
        product.ProductPricePerUnit = price;
        await AddProductAsync(product);
        return product;
    }

    // Methode: Neues Produkt hinzufügen
    private async Task AddProductAsync(Product product)
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
    
    public async Task addProductReviewAsync(int id, int rating, string? review, int userId)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await _context.reviews.AddAsync(new Review
        {
            ProductId = id,
            CustomerId = userId,
            Rating = rating,
            Comment = review,
            UserDisplayName = context.users.Where(u => u.CustomerId == userId).FirstOrDefault().DisplayName ?? throw new InvalidOperationException()

        });
        await _context.SaveChangesAsync();
    }
    
    public async Task updateProductReviewAsync(int id, int rating, string? review, int userId)
    {
        var reviewToUpdate = await _context.reviews.FirstOrDefaultAsync(r => r.ProductId == id && r.CustomerId == userId);
        if (reviewToUpdate != null)
        {
            reviewToUpdate.Rating = rating;
            reviewToUpdate.Comment = review;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeleteProductReviewAsync(int productId, int userId)
    {
        var review = await _context.reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.CustomerId == userId);
        if (review != null)
        {
            _context.reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
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
            using var bitmap = SKBitmap.Decode(filePath);
            {
                product.ImageWidth = bitmap.Width.ToString();
                product.ImageHeight = bitmap.Height.ToString();
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
        if (inventory == null)
        {
            throw new InvalidOperationException("Inventory not found");
        } else if (inventory.Quantity == null)
        {
            product.Stock = (int)inventory.Quantity;   
        }
        
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