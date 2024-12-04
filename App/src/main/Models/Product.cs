using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Webshop.Models.Products;

[Table("products")]
public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public decimal? ProductPrice { get; set; }
}