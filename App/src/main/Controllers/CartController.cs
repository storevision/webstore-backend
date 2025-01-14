using Microsoft.AspNetCore.Mvc;

namespace Webshop.App.src.main.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        [HttpPost("add")]
        public IActionResult AddToCart()
        {
            // Logik wird später hinzugefügt
            return Ok("Add to cart endpoint hit.");
        }

        [HttpPost("remove")]
        public IActionResult RemoveFromCart()
        {
            // Logik wird später hinzugefügt
            return Ok("Remove from cart endpoint hit.");
        }

        [HttpGet("list")]
        public IActionResult ListCartItems()
        {
            // Logik wird später hinzugefügt
            return Ok("List cart items endpoint hit.");
        }

        [HttpPost("checkout")]
        public IActionResult CheckoutCart()
        {
            // Logik wird später hinzugefügt
            return Ok("Checkout cart endpoint hit.");
        }

        [HttpPost("clear")]
        public IActionResult ClearCart()
        {
            // Logik wird später hinzugefügt
            return Ok("Clear cart endpoint hit.");
        }
    }
}