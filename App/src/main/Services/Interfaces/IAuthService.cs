using System.Security.Claims;
using Webshop.App.src.main.Models;

namespace Webshop.App.src.main.Services.Interfaces;

public interface IAuthService
{
    string GenerateToken(User user, bool keepLoggedIn);
    
    ClaimsPrincipal? ValidateToken(string token);
    
    void SetTokenCookie(HttpRequest request, HttpResponse response, string token, bool keepLoggedIn);
    
    void ClearTokenCookie(HttpResponse response);
    
    Task<TokenUser?> VerifyRequestAsync(HttpRequest request, HttpResponse response);
    
    void GenerateJwtTocken(HttpRequest request, HttpResponse response, User existingUser, bool keepLoggedIn);
}