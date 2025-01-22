using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Webshop.App.src.main.Models;

[Table("reviews")]
public class Review
{
    [NotMapped]
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Column("user_id")]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public User User { get; set; }
    
    [Column("created_at")]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [Column("comment")]
    public string Comment { get; set; }
    
    [Column("rating")]
    public decimal Rating { get; set; }
    
    //Unmaped propertys
    [NotMapped]
    [JsonPropertyName("user_display_name")]
    public string UserDisplayName { get; set; }
    [NotMapped]
    [JsonPropertyName("user_picture_data_url")]
    public string UserPictureDataUrl { get; set; }
    
}