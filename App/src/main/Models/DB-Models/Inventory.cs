using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("inventory")]
public class Inventory
{
    [Key]
    [Column("id")]
    public int InventoryId { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}