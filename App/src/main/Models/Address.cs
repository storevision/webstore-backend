using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Webshop.App.src.main.Models;
[Table("addresses")]
public class Address
{
    [Key]
    [Required]
    [Column("addressid")]
    [JsonPropertyName("addressid")]
    public int Addressid { get; set; }
    
    [Required]
    [Column("state")]
    [JsonPropertyName("state")]
    public string State { get; set; }
    
    [Column("country")]
    [JsonPropertyName("country")]
    public string Country { get; set; }
    
    [Column("region")]
    [JsonPropertyName("region")]
    public string Region { get; set; }
    
    [Required]
    [Column("zipcode")]
    [JsonPropertyName("zipcode")]
    public string ZipCode { get; set; }
    
    [Column("phone")]
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    
    [ForeignKey("CustomerId")]
    private int UserId { get; set; }
}