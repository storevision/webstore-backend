using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }
    
    [Column("total_amount")]
    public decimal? TotalAmount { get; set; }
    
    [Column("order_date")]
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

    public void setTotalAmount(decimal? totalAmount)
    {
        this.TotalAmount = totalAmount;
    }

}



