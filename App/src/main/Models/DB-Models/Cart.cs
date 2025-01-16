using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("carts")]
public class Cart
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    public Cart(int userId, int productId, int quantity)
    {
        UserId = userId;
        ProductId = productId;
        Quantity = quantity;
    }
    
}