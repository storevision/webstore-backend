using System.Net;
using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models.Responses;

namespace Webshop.App.src.main.Models.ApiHelper;

//ApiHelper class is a base class for all controllers in the project
public class ApiHelper : ControllerBase
{
    // is used to send a success response
    protected OkObjectResult SendSuccess<T>(T successData)
    {
        var response = new SuccessResponse<T>(successData);

        return Ok(response);
    }

    // is used to send an error response
    protected ObjectResult SendError(HttpStatusCode statusCode, string errorMessage)
    {
        var response = new ErrorResponse(errorMessage);
        
        ObjectResult result = new ObjectResult(response);
        result.StatusCode = (int)statusCode;
        
        return result;
    }
    
}