using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Webshop.App.src.main.Models;

public class Category
{
    [Key]
    public int CategoryID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}