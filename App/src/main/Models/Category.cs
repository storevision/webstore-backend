using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Webshop.App.src.main.Models;

[Table("categories")]
public class Category
{
    [Key]
    [Column("id")]
    public int CategoryId { get; set; }
    [Required]
    [Column("name")]
    public string Name { get; set; }
}