using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Webshop.App.src.main.Models;

namespace Webshop.Models.Products;

[Table("products")]
public class Product
{
    [Key]
    [Column("id")]
    public int ProductId { get; set; }
    
    [Required]
    [Column("name")]
    public string ProductName { get; set; }
    
    [Required]
    [Column("description")]
    public string? ProductDescription { get; set; }
    
    [Column("image_url")]
    public string ProductImage { get; set; }
    
    [Column("blurred_image")]
    public string ProductBlurredImage { get; set; }
    
    [Column("blurred_image_width")]
    public int ProductBlurredImageWidth { get; set; }
    
    [Column("blurred_image_height")]
    public int ProductBlurredImageHeight { get; set; }
    
    [Required]
    [Column("price_per_unit", TypeName = "decimal(5, 2)")]
    public decimal ProductPricePerUnit { get; set; }
    
    [ForeignKey("CategoryID")]
    public Category Category { get; set; }
}
