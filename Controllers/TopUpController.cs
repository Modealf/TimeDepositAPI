using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    [Authorize] // Ensure only authenticated users can access these endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class TopUpController(ITopUpService topUpService) : ControllerBase
    {
        // Helper method to get authenticated user's ID
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [HttpGet()]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserId();
            var balance = await topUpService.GetWalletBalanceAsync(userId);

            return Ok(new Walletbalance { Balance = balance });
        }


        [HttpPost()]
        public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequestDto request)
        {
            try
            {
                int userId = GetUserId();
                var topUp = await topUpService.TopUpAccountAsync(userId, request);
                return Ok(new TopUpResponse {Message = topUp});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went wrong");
            }
        }
    }
}