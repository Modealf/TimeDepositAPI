using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    [Authorize]  // Ensure only authenticated users can access these endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController(IDepositService depositService) : ControllerBase
    {

        // Helper method to get authenticated user's ID
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
        
        [Authorize]
        [HttpPost("request-custom")]
        public async Task<IActionResult> RequestCustomDeposit([FromBody] CreateCustomDepositRequestDto request)
        {
            int userId = GetUserId();
            var deposit = await depositService.RequestCustomDepositAsync(userId, request);
            return Ok(deposit);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDeposits()
        {
            int userId = GetUserId();
            var deposits = await depositService.GetUserDepositsAsync(userId);
            return Ok(deposits);
        }

        [HttpGet("Available")]
        public async Task<IActionResult> GetAvailableDeposits()
        {
            var crowdDepositOffers = await depositService.GetAvailableCrowdDepositsAsync();
            var result = new GetAvailableCrowdDepositsResponseDto { CrowdDepositOffers = crowdDepositOffers.ToList() };
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeposit(int id)
        {
            int userId = GetUserId();
            var deposit = await depositService.GetDepositByIdAsync(id, userId);
            if (deposit == null)
                return NotFound();
            return Ok(deposit);
        }

        [Authorize]
        [HttpPost("Crowd/enroll")]
        public async Task<IActionResult> EnrollInCrowdDeposit([FromBody] EnrollCrowdDepositRequestDto request)
        {
            int userId = GetUserId();
            await depositService.EnrollInCrowdDepositAsync(userId, request);
            return Ok("Enrolled");
        }
    }
}