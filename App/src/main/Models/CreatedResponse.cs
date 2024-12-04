using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Webshop.App.src.main.Models;

public class CreatedResponse<T>
{
    public bool success { get; set; }
    public List<T> data { get; set; }
    
    public void createResponse(bool status, List<T> objList)
    {
        success = status;
        data = objList;
    }
}

