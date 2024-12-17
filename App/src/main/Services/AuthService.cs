﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Services;

public class AuthService
{
    private readonly string _jwtSecret;
    private readonly IUserService _userService;

    public AuthService(IConfiguration configuration, IUserService userService)
    {
        _jwtSecret = configuration["JWT_SECRET"] ?? throw new Exception("JWT_SECRET is not defined");
        _userService = userService;
    }

    public string GenerateToken(User user, bool keepLoggedIn)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.CustomerID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("display_name", user.DisplayName)
            }),
            Expires = keepLoggedIn ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

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

    public void SetTokenCookie(HttpResponse response, string token, bool keepLoggedIn)
    {
        response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Requires HTTPS
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
}

public class TokenUser
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}