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
    public int addressid;
    
    [Required]
    [Column("state")]
    [JsonPropertyName("state")]
    public string state;
    
    [Column("country")]
    [JsonPropertyName("country")]
    public string country = "";
    
    [Column("region")]
    [JsonPropertyName("region")]
    public string region = "";
    
    [Required]
    [Column("zipcode")]
    [JsonPropertyName("zipcode")]
    public string zipcode;
    
    [Column("phone")]
    [JsonPropertyName("phone")]
    public string phone = "";
    
    [ForeignKey("CustomerId")]
    private int userid;
}