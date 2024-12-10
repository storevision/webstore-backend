using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Webshop.Services;

namespace Webshop.Controllers;

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
        _userService.CreateUser(firstName, lastName, email, phoneNumber, address, password);
        return Ok();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
    {
        bool loginOk = _userService.Login(email, password);

        if (loginOk)
        {
            return Ok("Login Ok");
        } else
        {
            return BadRequest("Login Fail");
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