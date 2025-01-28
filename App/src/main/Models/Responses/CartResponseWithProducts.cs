using System.Text.Json.Serialization;

namespace Webshop.App.src.main.Models.Responses;

public class CartResponseWithProducts
{
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("product")]
    public Product Product { get; set; }
}