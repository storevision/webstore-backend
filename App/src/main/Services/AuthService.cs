using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Services;
using Webshop.App.src.main.Services.Interfaces;

// AuthService is a service class that handles authentication
public class AuthService : IAuthService
{
    // injects the configuration and user service
    private readonly string _jwtSecret;
    private readonly IUserService _userService;

    // when the AuthService is created, the configuration and user service are injected
    public AuthService(IConfiguration configuration, IUserService userService)
    {
        _jwtSecret = configuration["JWT_SECRET"] ?? throw new Exception("JWT_SECRET is not defined");
        _userService = userService;
    }
    
    // GenerateToken method is used to generate a JWT token
    public string GenerateToken(User user, bool keepLoggedIn)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.CustomerId.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException("No email found")),
                new Claim("display_name", user.DisplayName ?? throw new InvalidOperationException("No display name found"))
            }),
            Expires = keepLoggedIn ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // ValidateToken method is used to validate a JWT token
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }

    public void SetTokenCookie(HttpRequest request, HttpResponse response, string token, bool keepLoggedIn)
    {
        response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = keepLoggedIn ? DateTimeOffset.UtcNow.AddDays(30) : null
        });
    }

    public void ClearTokenCookie(HttpResponse response)
    {
        response.Cookies.Delete("token");
    }

    public async Task<TokenUser?> VerifyRequestAsync(HttpRequest request, HttpResponse response)
    {
        if (!request.Cookies.TryGetValue("token", out var token) || string.IsNullOrEmpty(token))
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            await response.WriteAsJsonAsync(new { success = false, message = "Unauthorized" });
            return null;
        }

        var claimsPrincipal = ValidateToken(token);

        if (claimsPrincipal == null)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            await response.WriteAsJsonAsync(new { success = false, message = "Unauthorized" });
            return null;
        }

        var user = new TokenUser
        {
            Id = claimsPrincipal.FindFirst("id")?.Value ?? string.Empty,
            Email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            DisplayName = claimsPrincipal.FindFirst("display_name")?.Value ?? string.Empty
        };

        if (!await _userService.VerifyUserByObjectAsync(user))
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            await response.WriteAsJsonAsync(new { success = false, message = "Unauthorized" });
            return null;
        }

        return user;
    }

    public void GenerateJwtTocken(HttpRequest request, HttpResponse response, User existingUser, bool keepLoggedIn )
    {
        var token = this.GenerateToken(existingUser, keepLoggedIn);

        // Set cookie
        this.SetTokenCookie(request, response, token, keepLoggedIn);
    }
    
}

public class TokenUser
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
