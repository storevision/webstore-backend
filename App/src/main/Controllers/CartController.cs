using Microsoft.AspNetCore.Mvc;
using Webshop.App.src.main.Models;
using Webshop.App.src.main.Models.ApiHelper;
using Webshop.App.src.main.Models.Responses;
using Webshop.App.src.main.Services;
using Webshop.Services;

namespace Webshop.App.src.main.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ApiHelper
    {
        
        private readonly CartService _cartService;
        private readonly AuthService _authService;
        private readonly IUserService _userService;
    
        public CartController(CartService cartService, AuthService authService, IUserService userService)
        {
            _cartService = cartService;
            _authService = authService;
            _userService = userService;
            
        }
        
        // need to configure the error requests
        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] ReviewRequestBody reviewRequestBody)
        {
            var userId = getUserId();
            _cartService.addArticleToCart(userId, reviewRequestBody.product_id, reviewRequestBody.quantity);
            Task<CartResponse[]> cartResponses = _cartService.getCartForUser(userId);
            return this.SendSuccess(cartResponses);
        }

        [HttpPost("remove")]
        public IActionResult RemoveFromCart([FromBody] ReviewRequestBody reviewRequestBody)
        {
            var userId = getUserId();
            _cartService.removeArticleFromCart(userId, reviewRequestBody.product_id, reviewRequestBody.quantity);
            Task<CartResponse[]> cartResponses = _cartService.getCartForUser(userId);
            return this.SendSuccess(cartResponses);
        }

        [HttpGet("list")]
        public IActionResult ListCartItems()
        {
            CartResponseWithProducts[] cartItems;
            var userId = getUserId();

            cartItems = _cartService.getCartForUserWithProducts(userId);
            
            return this.SendSuccess(cartItems);
        }

        [HttpPost("checkout")]
        public IActionResult CheckoutCart([FromBody] Address address)
        {
            bool success = true;
            return Ok(success);
        }

        [HttpPost("clear")]
        public IActionResult ClearCart()
        {
            // Logik wird später hinzugefügt
            return Ok("Clear cart endpoint hit.");
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
        
        public class ReviewRequestBody
        {
            public int product_id { get; set; }
            public int quantity { get; set; }
        }
        
    }
}