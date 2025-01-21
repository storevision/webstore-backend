using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("order_items")]
public class OrderDetails
{
    [Column("order_id")]
    public int OrderId { get; set; }
    [ForeignKey("OrderId")] 
    public Order Order { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("price_per_unit")]
    public decimal UnitPrice { get; set; }
    
}

