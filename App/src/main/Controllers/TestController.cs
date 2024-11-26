using Microsoft.AspNetCore.Mvc;

namespace Webshop.Controllers
{
        [ApiController] // Kennzeichnet die Klasse als API-Controller
        [Route("test")] // Basis-Route: test
        public class TestController : ControllerBase
        {
            [HttpPost]
            public IActionResult Create([FromBody] String text)
            {
                string responseMessage = $@" <html> <head> <title>Message</title> </head> <body> <p> the server says: {text} </p> </body> </html> "; 
                return Ok(responseMessage);
                //return Created("", new { message = textMessage, data });
            }
        }
}