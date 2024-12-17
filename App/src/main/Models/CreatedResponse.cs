using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Webshop.App.src.main.Models;

public class CreatedResponse<T>
{
    public required bool success { get; set; }
    public List<T> data { get; set; }
    public T? result { get; set; }
    
    public string? error { get; set; }
    
    public void createSuccessListResponse(bool status, List<T> obj)
    {
        success = status;
        if (status)
        {
            data = obj;
        }
    }
    public void createSuccessResponse(bool status, T obj)
    {
        success = status;
        if (status)
        {
            result = obj;
        }
    }

    public void createErrorResponse(bool status, string err)
    {
        success = status;
        error = err;
    }
    
}

