using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.ApiHelper;
using Webshop.App.src.main.Models.Responses;
using Webshop.App.src.main.Services;
using Webshop.App.src.main.Services.Interfaces;

namespace Webshop.App.src.main.Controllers;

// UserController class is a controller class for user related operations
[ApiController]
[Route("users")]
public class UserController : ApiHelper
{
    // here we inject the IUserService and IAuthService interfaces
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    //when the UserController is created, the IUserService and IAuthService are injected
    public UserController(IUserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    // CreateUser method is used to create a new user
    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequestBody user)
    {
        await _userService.CreateUserAsync(new User(user.email, user.password, user.displayName));

        var registeredUser = await _userService.GetUserByEmailAsync(user.email);

        if (registeredUser != null)
        {
            _authService.GenerateJwtTocken(Request, Response, registeredUser, false);
        }
        else
        {
            return this.SendError(HttpStatusCode.InternalServerError, "User could not be created.");
        }

        return this.SendSuccess(new UserResponseData(registeredUser));
    }

    // Login method is used to login a user
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody user)
    {
        
        var existingUser = await _userService.GetUserByEmailAsync(user.email);
        
        if (existingUser == null || !existingUser.VerifyPassword(user.password))
        {
            return this.SendError(HttpStatusCode.Unauthorized, "Invalid email or password.");
        }
        // Generate JWT token
        _authService.GenerateJwtTocken(Request, Response, existingUser, user.keepLoggedIn);
        
        return this.SendSuccess(new UserResponseData(existingUser));
    }
    
    // Logout method is used to logout a user
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try {
            _authService.ClearTokenCookie(Response);
            return Ok(new {success = true});
        } catch {
            return this.SendError(HttpStatusCode.InternalServerError, "Logout failed.");
        }
        
    }
    
    // Info method is used to get user information
    [HttpGet("info")]
    public IActionResult Info()
    {
        User requiredUser;
        var token = Request.Cookies["token"];

        if (string.IsNullOrEmpty(token))
        {
            return this.SendError(HttpStatusCode.Unauthorized, "Token missing or invalid.");
        }

        var userClaims = _authService.ValidateToken(token);

        if (userClaims == null)
        {
            return this.SendError(HttpStatusCode.Unauthorized, "Token validation failed.");
        }

        int value = Convert.ToInt32(userClaims.FindFirst("id")?.Value);
        Task<User?> user;
        
        
        user = _userService.GetUserByIdAsync(value) ?? throw new InvalidOperationException("User not found.");
        requiredUser = user.Result ?? throw new InvalidOperationException("User not found.");
        
        // return user information as response
        return this.SendSuccess(new UserResponseData(requiredUser));
    }
    
    [HttpGet("settings")]
    public IActionResult Settings()
    {
        var userId = getUserId();
        if (userId == -1)
        {
            return this.SendError(HttpStatusCode.Unauthorized, "Token missing or invalid.");
        }
        UserAddressResponse response = new UserAddressResponse
        {
            Addresses = _userService.getUserAdresses(userId)
        };
        return this.SendSuccess(response);
    }
    
    [HttpPost("settings")]
    public IActionResult Settings([FromBody] UserAddressResponse addressList)
    {
        var userId = getUserId();
        if (userId == -1)
        {
            return this.SendError(HttpStatusCode.Unauthorized, "Token missing or invalid.");
        }
        var address = addressList.Addresses[0];
        _userService.addUserAddress(userId, address);
        return this.SendSuccess(new {success = true});
    }
    
    private int getUserId()
    {
        var token = Request.Cookies["token"];
        var userClaims = _authService.ValidateToken(token);
            
        try
        {
            if (userClaims == null)
            {
                throw new Exception("Token validation failed.");
            }
        }
        catch (Exception e)
        {
            return -1;
        }
        return Convert.ToInt32(userClaims.FindFirst("id")?.Value);
    }
    
    public class RegisterRequestBody
    {
        public string email { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }
    }

    public class LoginRequestBody
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool keepLoggedIn { get; set; }
    }

    public class UserAddressResponse
    {
        [JsonPropertyName("addresses")]
        public required Address[] Addresses { get; set; }
        
    }
}
