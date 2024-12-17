using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Services;

namespace Webshop.App.src.main.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly AuthService _authService;

    public UserController(IUserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequestBody user)
    {
        var newUser = new User(user.email, user.password, user.displayName);
        await _userService.CreateUserAsync(newUser);
        
        var registeredUser = await _userService.GetUserByEmailAsync(user.email);

        if (registeredUser != null) generateJwtTocken(registeredUser, false);
        
        var response = new
        {
            success = true,
            data = new
            {
                id = newUser.CustomerID,
                email = newUser.Email,
                display_name = newUser.DisplayName
            }
        };
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody user)
    {
        
        var existingUser = await _userService.GetUserByEmailAsync(user.email);

        var createdResponse = new CreatedResponse<User>
        {
            success = true
        };
        if (existingUser == null || !existingUser.VerifyPassword(user.password))
        {
            createdResponse.createErrorResponse(false, "Password is incorrect.");
            return BadRequest(createdResponse);
        }

        // Generate JWT token
        generateJwtTocken(existingUser, user.keepLoggedIn);
        var response = new
        {
            success = true,
            data = new
            {
                id = existingUser.CustomerID,
                email = existingUser.Email,
                display_name = existingUser.DisplayName
            }
        };
        
        return Ok(response);
    }

    [HttpPost]
    [Route("logout")]
    public IActionResult Logout()
    {
        _authService.ClearTokenCookie(Response);
        return Ok(new { success = true });
    }

    [HttpGet]
    [Route("info")]
    public IActionResult Info()
    {
        var token = Request.Cookies["token"];
        var createdResponse = new CreatedResponse<User?>
        {
            success = true
        };

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Token missing or invalid." });
        }

        var userClaims = _authService.ValidateToken(token);

        if (userClaims == null)
        {
            return Unauthorized(new { message = "Token validation failed." });
        }

        int value = Convert.ToInt32(userClaims.FindFirst("id")?.Value);
        Task<User?> user = null;
        
        if (value != null)
        {
            user = _userService.GetUserByIdAsync(value);
            User requiredUser = user.Result;
            createdResponse.createSuccessResponse(true ,requiredUser);
        }

        var response = new
        {
            success = true,
            data = new
            {
                id = Convert.ToInt32(userClaims.FindFirst("id")?.Value),
                email = userClaims.FindFirst("email")?.Value,
                display_name = userClaims.FindFirst("display_name")?.Value
            }
        };

        // Rückgabe als JSON
        return Ok(response);
    }

    private void generateJwtTocken(User existingUser, bool keepLoggedIn )
    {
        var token = _authService.GenerateToken(existingUser, keepLoggedIn);

        // Set cookie
        _authService.SetTokenCookie(Response, token, keepLoggedIn);
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
}
