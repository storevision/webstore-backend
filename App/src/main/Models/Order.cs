using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webshop.App.src.main.Models;
using Webshop.Models.Products;

namespace Webshop.Models.Cart;

[Table("orders")]
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public float? TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    [ForeignKey("CustomerId")]
    public int CustomerId { get; set; }

    public ICollection<OderDetails> OrderDetails { get; set; } = null!;
    
    
    // The int Variable from the first dictionary is used to store the Product Id
    // the int in the inner dictionary is used for the quantity.
    [NotMapped]
    public Dictionary<int, Dictionary<int, Product>> articleList { get; set; }
    
    public Order()
    {
        articleList = new Dictionary<int, Dictionary<int, Product>>();
    }
    
    public void AddArticleToList(Product product, int quantity) {
        
        Dictionary<int , Product> article = new Dictionary<int , Product>();
        article.Add(quantity, product);
        int id = product.ProductId;
        articleList.Add(id, article);
        
    }

    public void setTotalAmount(float? totalAmount)
    {
        this.TotalAmount = totalAmount;
    }

}



