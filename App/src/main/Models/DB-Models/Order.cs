using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Azure.Core.Serialization;

namespace Webshop.App.src.main.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    [JsonPropertyName("id")]
    public int OrderId { get; set; }
    
    [Column("total_amount")]
    [JsonIgnore]
    public decimal? TotalAmount { get; set; }
    
    [JsonPropertyName("created_at")]
    [Column("order_date")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [JsonIgnore]
    [Column("user_id")] 
    public int CustomerId { get; set; } 
    // FK-Eigenschaft
    [JsonIgnore]
    [ForeignKey("CustomerId")]
    public User? User { get; set; }
    
    [Column("address_id")]
    [JsonIgnore]
    public int AddressId { get; set; }
    
    [JsonPropertyName("address")]
    [ForeignKey("AddressId")]
    public Address? Address { get; set; }
    
    // The int Variable from the first dictionary is used to store the Product Id
    // the int in the inner dictionary is used for the quantity.
    [NotMapped]
    [JsonPropertyName("items")]
    public List<Item> Items { get; set; } = new List<Item>();

    public void setTotalAmount(decimal? totalAmount)
    {
        this.TotalAmount = totalAmount;
    }
    
    public class Item()
    {
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        
        [JsonPropertyName("price_per_unit")]
        public decimal Price { get; set; }

        public Item(int productId, int quantity, decimal price) : this()
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}





