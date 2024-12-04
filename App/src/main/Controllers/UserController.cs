using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Webshop.Services;

namespace Webshop.Controllers;

[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{

    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> CreateUser([FromForm] string email, [FromForm] string password, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string address, [FromForm] string phoneNumber)
    {
        _userService.CreateUser(firstName, lastName, email, phoneNumber, address, password);
        return Ok();
    }
    
    [HttpPost]
    [Route("Login")]
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
    
    
}