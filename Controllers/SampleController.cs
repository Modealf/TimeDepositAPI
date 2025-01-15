using Microsoft.AspNetCore.Mvc;

namespace TimeDepositAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class SampleController : ControllerBase
    {
        // A simple GET endpoint
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello from SampleController!" });
        }
    }
}
