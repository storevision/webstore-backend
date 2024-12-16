using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Webshop.App.src.main.Models;

namespace Webshop.Models.Products;

[Table("products")]
public class Product
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int ProductId { get; set; }
    
    [Required]
    [Column("name")]
    [JsonPropertyName("name")]
    public string ProductName { get; set; }
    
    [Required]
    [Column("description")]
    [JsonPropertyName("description")]
    public string? ProductDescription { get; set; }
    
    [Column("image_url")]
    [JsonPropertyName("image_url")]
    public string ProductImage { get; set; }
    
    [Column("blurred_image")]
    [JsonPropertyName("blurred_image")]
    public string? ProductBlurredImage { get; set; }
    
    [Column("blurred_image_width")]
    [JsonPropertyName("blurred_image_width")]
    public int? ProductBlurredImageWidth { get; set; }
    
    [Column("blurred_image_height")]
    [JsonPropertyName("blurred_image_height")]
    public int? ProductBlurredImageHeight { get; set; }
    
    [Required]
    [Column("price_per_unit", TypeName = "decimal(5, 2)")]
    [JsonPropertyName("price_per_unit")]
    public decimal ProductPricePerUnit { get; set; }
    
    [JsonPropertyName("category_id")]
    [Column("category_id")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [JsonIgnore]
    public Category Category { get; set; }
    
}
