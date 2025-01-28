using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.ApiHelper;
using Webshop.App.src.main.Services;
using Webshop.App.src.main.Services.Interfaces;

namespace Webshop.App.src.main.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ApiHelper
    {
        private readonly OrderService _orderService;
        private readonly IAuthService _authService;
        
        public OrderController(OrderService orderService, AuthService authService)
        {
            _orderService = orderService;
            _authService = authService;
        }
        
        
        [HttpGet("list")]
        public IActionResult ListOrders()
        {
            Order[] orders = _orderService.ListOrders(getUserId()).ToArray();
            return this.SendSuccess(orders);
        }
        // need a id to get the order
        [HttpGet("get/{id}")]
        public IActionResult GetOrder()
        {
            // Logik wird später hinzugefügt
            return Ok("Get order endpoint hit.");
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
    }
}