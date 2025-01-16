using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;

namespace Webshop.App.src.main.Models;
[Table("addresses")]
public class Address
{
    [Key]
    [Required]
    [Column("address_id")]
    [JsonIgnore]
    public int Addressid { get; set; }
    
    [Column("address")]
    [JsonPropertyName("address")]
    public string Street { get; set; }
    
    [Column("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("state")]
    [JsonPropertyName("state")]
    public string State { get; set; }
    
    [Column("country")]
    [JsonPropertyName("country")]
    public string Country { get; set; }
    
    [Column("city")]
    [JsonPropertyName("city")]
    public string City { get; set; }
    
    [Required]
    [Column("postal_code")]
    [JsonPropertyName("postal_code")]
    public string PostalCode { get; set; }
    
    
    [ForeignKey("user_id")] 
    [Column("user_id")]
    public int UserId { get; set; } 
    [JsonIgnore] 
    public User User { get; set; }
}