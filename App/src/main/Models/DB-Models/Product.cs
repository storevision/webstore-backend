using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Webshop.App.src.main.Models;

namespace Webshop.App.src.main.Models;

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
    
    [JsonPropertyName("image_with")]
    [Column("image_with")]
    public string? ImageWith { get; set; }
    
    [JsonPropertyName("image_height")]
    [Column("image_height")]
    public string? ImageHeight { get; set; }
    
    [JsonPropertyName("stock")]
    [Column("stock")]
    public int? Stock { get; set; }
    
    [JsonPropertyName("one_star")]
    [Column("one_star")]
    public int? OneStar { get; set; }
    
    [JsonPropertyName("two_star")]
    [Column("two_star")]
    public int? TwoStar { get; set; }
    
    [JsonPropertyName("three_star")]
    [Column("three_star")]
    public int? ThreeStar { get; set; }
    
    [JsonPropertyName("four_star")]
    [Column("four_star")]
    public int? FourStar { get; set; }
    
    [JsonPropertyName("five_star")]
    [Column("five_star")]
    public int? FiveStar { get; set; }
    
    [JsonPropertyName("total_reviews")]
    [Column("total_reviews")]
    public int? TotalReviews { get; set; }
    
    [JsonPropertyName("average_rating")]
    [Column("average_rating")]
    public int? AverageRating { get; set; }
}
