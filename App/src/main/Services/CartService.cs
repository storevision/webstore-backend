using System.Security.Cryptography;
using Webshop.Models.Cart;
using Webshop.Models.Products;


namespace Webshop.Services;

public class CartService
{
    //Add an Article to the card
    public static void addArticleToCart(int quantity, String productId)
    
    {
        Product product = new Product(); // TODO waiting for the CRUD management from @David
        
        Order order = new Order();
        order.AddArticleToList(product, quantity);

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
                amount += article.Value.ProductPrice * article.Key;
            }
            
        }
        order.setTotalAmount(amount);
    }
}