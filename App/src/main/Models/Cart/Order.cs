using Webshop.Models.Products;

namespace Webshop.Models.Cart;

public class Order
{
    public string orderId { get; set; }
    public float sum { get; set; }
    public DateTime date { get; set; }
    Dictionary<int, Dictionary<int, object>> articleList { get; set; }
    
    public void AddArticleToList(Product product, int quantity) {
        
        Dictionary<int , object> article = new Dictionary<int , object>();
        article.Add(quantity, product);
        
        articleList.Add(product.productid, article);
        
    }

}



