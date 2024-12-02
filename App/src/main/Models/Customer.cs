using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Models.Cart;

namespace Webshop.App.src.main.Models;

[Table("customers")]
public class Customer
{
    [Key]
    public int CustomerID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public ICollection<Order>? Orders { get; set; } = null!;
}