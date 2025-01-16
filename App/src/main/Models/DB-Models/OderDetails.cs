using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("orderDetails")]
public class OderDetails
{
    [Key]
    public int OrderDetailId { get; set; }
    
    public int OrderId { get; set; }
    [ForeignKey("OrderId")] 
    public Order Order { get; set; }

    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
}

