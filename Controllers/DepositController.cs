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
    /// <summary>
    /// Controller for managing deposit operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController(IDepositService depositService) : ControllerBase
    {

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
        
        /// <summary>
        /// Request a custom deposit with specific terms
        /// </summary>
        /// <param name="request">Custom deposit request details</param>
        /// <returns>Created custom deposit request</returns>
        /// <response code="200">Custom deposit request created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">User not authenticated</response>
        [Authorize]
        [HttpPost("request-custom")]
        public async Task<IActionResult> RequestCustomDeposit([FromBody] CreateCustomDepositRequestDto request)
        {
            int userId = GetUserId();
            var deposit = await depositService.RequestCustomDepositAsync(userId, request);
            return Ok(deposit);
        }

        /// <summary>
        /// Get all deposits for the authenticated user
        /// </summary>
        /// <returns>List of user's deposits</returns>
        /// <response code="200">Deposits retrieved successfully</response>
        /// <response code="401">User not authenticated</response>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDeposits()
        {
            int userId = GetUserId();
            var deposits = await depositService.GetUserDepositsAsync(userId);
            return Ok(deposits);
        }

        /// <summary>
        /// Get all available crowd deposit offers
        /// </summary>
        /// <returns>List of available crowd deposit offers</returns>
        /// <response code="200">Available deposits retrieved successfully</response>
        [HttpGet("Available")]
        public async Task<IActionResult> GetAvailableDeposits()
        {
            var crowdDepositOffers = await depositService.GetAvailableCrowdDepositsAsync();
            var result = new GetAvailableCrowdDepositsResponseDto { CrowdDepositOffers = crowdDepositOffers.ToList() };
            return Ok(result);
        }

        /// <summary>
        /// Get a specific deposit by ID for the authenticated user
        /// </summary>
        /// <param name="id">Deposit ID</param>
        /// <returns>Deposit details</returns>
        /// <response code="200">Deposit found and returned</response>
        /// <response code="404">Deposit not found</response>
        /// <response code="401">User not authenticated</response>
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

        /// <summary>
        /// Enroll in an available crowd deposit offer
        /// </summary>
        /// <param name="request">Enrollment details</param>
        /// <returns>Confirmation of enrollment</returns>
        /// <response code="200">Successfully enrolled in crowd deposit</response>
        /// <response code="400">Invalid enrollment request</response>
        /// <response code="401">User not authenticated</response>
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