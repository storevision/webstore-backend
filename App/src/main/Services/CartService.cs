using Microsoft.EntityFrameworkCore;
using Webshop.Models.DB;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.Responses;


namespace Webshop.Services;

public class CartService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;

    public CartService(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _scopeFactory = scopeFactory;
    }
    
    //Add an Article to the card
    public void addArticleToCart(int userId, int product_id, int quantity)
    {
        Cart cart = new Cart(userId, product_id, quantity);
        
        if (checkifProductAlreadyInCart(userId, product_id))
        {
            _context.Database.ExecuteSqlRaw("UPDATE carts SET quantity = quantity + {0} WHERE user_id = {1} AND product_id = {2}", quantity, userId, product_id);
            _context.SaveChangesAsync();
        }
        else
        {
            _context.carts.Add(cart);
            _context.SaveChangesAsync();
        }
    }

    public Product getProduct(int productId)
    {
        Product product = new Product();
        var dbProduct = _context.products.FromSqlRaw("SELECT * FROM products WHERE id = {0}", productId);
        foreach (var p in dbProduct)
        {
            product = p;
        }
        return product;
    }
    
    public async Task<Cart[]> getCart(int id)
    { 
        using var scope = _scopeFactory.CreateScope(); // Erstelle einen neuen Scope
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Cart[] carts = await dbContext.carts.FromSqlRaw("SELECT * FROM carts WHERE user_id = {0}", id).ToArrayAsync();
        return carts;
    }


    public CartResponseWithProducts[] getCartItems(Cart[] cart)
    {
        CartResponseWithProducts[] cartItems = new CartResponseWithProducts[cart.Length];
        for (int i = 0; i < cart.Length; i++)
        {
            cartItems[i] = new CartResponseWithProducts();
            cartItems[i].ProductId = cart[i].ProductId;
            cartItems[i].Quantity = cart[i].Quantity;
            cartItems[i].Product = getProduct(cart[i].ProductId);
            cartItems[i].Product.Stock = _context.inventory.FromSqlRaw("SELECT * FROM inventory WHERE product_id = {0}", cart[i].ProductId).FirstOrDefault().Quantity;
        }

        return cartItems;
    }

    public void removeArticleFromCart(int userId, int productId, int quantity)
    {
        if (quantity > 0)
        {
            _context.Database.ExecuteSqlRaw("UPDATE carts SET quantity = quantity - {0} WHERE user_id = {1} AND product_id = {2}", quantity, userId, productId);
        }
        else
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM carts WHERE user_id = {0} AND product_id = {1}", userId, productId);
        }
        
    }

    public CartResponseWithProducts[] getCartForUserWithProducts(int userId)
    {
        CartResponseWithProducts[] cartItems;
        
        Console.WriteLine(userId);
        var cart = this.getCart(userId);

        cartItems = this.getCartItems(cart.Result);
        //Die Liste muss sortiert sein so das sie nach dem Produkt sortiert ist
        Array.Sort(cartItems, (x, y) => x.Product.ProductId.CompareTo(y.Product.ProductId));
        
        return cartItems;
    }

    public async Task<CartResponse[]> getCartForUser(int userId)
    {
        List<CartResponse> cartItems = new List<CartResponse>();
        var carts = await this.getCart(userId);
        foreach (var cart in carts)
        {
            CartResponse cartResponse = new CartResponse();
            cartResponse.ProductId = cart.ProductId;
            cartResponse.Quantity = cart.Quantity;
            cartItems.Add(cartResponse);
        }
        return cartItems.ToArray();
    }

    public void removeCartFromDb(int getUserId)
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM carts WHERE user_id = {0}", getUserId);
    }
    
    private bool checkifProductAlreadyInCart(int userId, int productId)
    {
        var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var cart = dbContext.carts.FromSqlRaw("SELECT * FROM carts WHERE user_id = {0} AND product_id = {1}", userId, productId).ToArray();
        return cart.Length > 0;
    }
}