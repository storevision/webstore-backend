using System.Text.Json.Serialization;

namespace Webshop.App.src.main.Models.Responses;

// generates a success response
public class SuccessResponse<T>(T responseData)
{
    public bool success => true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // ignores if the value is null
    public T data { get; set; } = responseData;
    
}