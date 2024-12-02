using Microsoft.AspNetCore.Mvc;
using Webshop.Services;

namespace Webshop.Controllers
{
    [ApiController] // Kennzeichnet die Klasse als API-Controller
    [Route("api/Webshop")] // Basis-Route: api/My
    public class WebshopController : ControllerBase
    {
        ProductService _productService;
        // GET: api/Webshop
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello, world!" });
        }

        // GET: api/Webshop/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new { id, message = $"You requested item {id}" });
        }

        // POST: api/Webshop
        [HttpPost]
        public IActionResult Create([FromBody] object data)
        {
            return Created("", new { message = "Item created!", data });
        }

        // STrg Z
    }
}