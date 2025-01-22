using System.Text.Json.Nodes;
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
        private readonly OrderService _orderService;
        private readonly IUserService _userService;
        
    
        public CartController(CartService cartService, AuthService authService, OrderService orderService, IUserService userService)
        {
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _userService = userService;
            
        }
        
        // need to configure the error requests
        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] CartRequestBody cartRequestBody)
        {
            var userId = getUserId();
            _cartService.addArticleToCart(userId, cartRequestBody.product_id, cartRequestBody.quantity);
            Task<CartResponse[]> cartResponses = _cartService.getCartForUser(userId);
            return this.SendSuccess(cartResponses);
        }

        [HttpPost("clear")]
        public IActionResult CLearCart([FromBody] CartRequestBody cartRequestBody)
        {
            var userId = getUserId();
            _cartService.removeArticleFromCart(userId, cartRequestBody.product_id, cartRequestBody.quantity);
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
        public IActionResult CheckoutCart([FromBody] AddressDto addressDto)
        {
            _orderService.createOrder(getUserId(), addressDto.address);
            _cartService.removeCartFromDb(getUserId());
            return Ok(new { success = true });
        }

        [HttpPost("remove")]
        public IActionResult RemoveFromCart([FromBody] CartRequestBody cartRequestBody)
        {
            var userId = getUserId();
            _cartService.removeArticleFromCart(userId, cartRequestBody.product_id, cartRequestBody.quantity);
            Task<CartResponse[]> cartResponses = _cartService.getCartForUser(userId);
            return this.SendSuccess(cartResponses);
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
        
        public class CartRequestBody
        {
            public int product_id { get; set; }
            public int quantity { get; set; }
        }
        
        public class AddressDto
        {
            public Models.Address address { get; set; }
        }
        
    }
}