using Microsoft.AspNetCore.Mvc;

namespace Webshop.App.src.main.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult ListOrders()
        {
            // Logik wird später hinzugefügt
            return Ok("List orders endpoint hit.");
        }
        // need a id to get the order
        [HttpGet("get/{id}")]
        public IActionResult GetOrder()
        {
            // Logik wird später hinzugefügt
            return Ok("Get order endpoint hit.");
        }
    }
}