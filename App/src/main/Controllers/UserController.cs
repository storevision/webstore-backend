using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Services;

namespace Webshop.App.src.main.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> CreateUser([FromForm] string email, [FromForm] string password, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string address, [FromForm] string phoneNumber)
    {
        _userService.CreateUser(email, password, displayName);
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
    {
        bool loginOk = _userService.Login(email, password);

        if (loginOk)
        {
            return Task.FromResult<IActionResult>(Ok("Login Ok"));
        } else
        {
            return Task.FromResult<IActionResult>(BadRequest("Login Fail"));
        }
    }
    
    [HttpGet]
    [Route("info")]
    public async Task<IActionResult> Info()
    {
        var response = new { message = "Login Ok" };
        return StatusCode(201, response);
    }
    
    
}
