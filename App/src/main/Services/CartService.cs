using Microsoft.EntityFrameworkCore;
using Webshop.Models.DB;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.Responses;


namespace Webshop.Services;

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //Add an Article to the card
    public async Task addArticleToCart(int userId, int product_id, int quantity)
    {
        
        Cart cart = new Cart(userId, product_id, quantity);

        _context.carts.Add(cart);
        await _context.SaveChangesAsync();

    }

    public static Product getProduct(String productId)
    {
        Product product = new Product();
        return product;
    }

    public static void calculateTotalPrice(Order order)
    {
        decimal? amount = 0;
        
        foreach (var articleAndQuantity in order.articleList )
        {
            foreach (var article in articleAndQuantity.Value)
            {
                amount += article.Value.ProductPricePerUnit * article.Key;
            }
            
        }
        order.setTotalAmount(amount);
    }
    
    public async Task<Cart[]> getCart(int id)
    { 
        Cart[] carts = await _context.carts.FromSqlRaw("SELECT * FROM carts WHERE user_id = {0}", id).ToArrayAsync();
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
            cartItems[i].Product = getProduct(cart[i].ProductId.ToString());
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
        return cartItems;
    }

    public CartResponse[] getCartForUser(int userId)
    {
        List<CartResponse> cartItems = new List<CartResponse>();
        var carts = this.getCart(userId);
        foreach (var cart in carts.Result)
        {
            CartResponse cartResponse = new CartResponse();
            cartResponse.ProductId = cart.ProductId;
            cartResponse.Quantity = cart.Quantity;
            cartItems.Add(cartResponse);
        }
        return cartItems.ToArray();
    }
}