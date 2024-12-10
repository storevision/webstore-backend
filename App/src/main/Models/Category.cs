using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Webshop.App.src.main.Models;

public class Category
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int CategoryID { get; set; }
    [Column("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [Column("description")]
    [JsonPropertyName("description")]
    [JsonIgnore]
    public string Description { get; set; }
}