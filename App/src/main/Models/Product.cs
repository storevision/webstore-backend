using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Webshop.Models.Products;

[Table("products")]
public class Product
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int ProductId { get; set; }
    [Column("name")]
    [JsonPropertyName("name")]
    public string ProductName { get; set; }
    [Column("description")]
    [JsonPropertyName("description")]
    public string? ProductDescription { get; set; }
    [Column("price_per_unit")]
    [JsonPropertyName("price_per_unit")]
    public decimal? ProductPrice { get; set; }
    [Column("image_url")]
    [JsonPropertyName("image_url")]
    
    public string? ProductImage { get; set; }
    [Column("category_id")]
    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }
}