using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Webshop.App.src.main.Models;

[Table("categories")]
public class Category
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int CategoryId { get; set; }
    [Required]
    [Column("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
