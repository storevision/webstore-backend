using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Webshop.Models.Products;

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
    
    [ForeignKey("product_id")]
    public Product Product { get; set; }
}