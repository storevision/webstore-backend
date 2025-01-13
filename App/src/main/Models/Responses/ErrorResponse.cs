using System.Text.Json.Serialization;

namespace Webshop.App.src.main.Models.Responses;

// generates an error response
public class ErrorResponse(string errorMessage)
{
    public ErrorResponse() : this("Internal Server Error")
    { }

    public bool success => false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // ignores if the value is null
    public string error { get; set; } = errorMessage;
}