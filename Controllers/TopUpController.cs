using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    /// <summary>
    /// Controller for managing wallet top-up operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TopUpController(ITopUpService topUpService) : ControllerBase
    {
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        /// <summary>
        /// Get the current wallet balance for the authenticated user
        /// </summary>
        /// <returns>Current wallet balance</returns>
        /// <response code="200">Balance retrieved successfully</response>
        /// <response code="401">User not authenticated</response>
        [HttpGet()]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserId();
            var balance = await topUpService.GetWalletBalanceAsync(userId);

            return Ok(new Walletbalance { Balance = balance });
        }

        /// <summary>
        /// Add funds to the authenticated user's wallet
        /// </summary>
        /// <param name="request">Top-up amount and details</param>
        /// <returns>Confirmation of successful top-up</returns>
        /// <response code="200">Top-up completed successfully</response>
        /// <response code="400">Invalid top-up request or amount</response>
        /// <response code="401">User not authenticated</response>
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