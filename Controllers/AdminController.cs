using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers;

/// <summary>
/// Controller for administrative operations - restricted to admin users only
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService adminService, IBackgroundJobClient backgroundJobClient) : ControllerBase
{
    /// <summary>
    /// Create a new crowd deposit offer
    /// </summary>
    /// <param name="createCrowdDepositOfferDto">Crowd deposit offer details</param>
    /// <returns>Created crowd deposit offer</returns>
    /// <response code="200">Crowd deposit offer created successfully</response>
    /// <response code="400">Invalid offer data</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have admin privileges</response>
    [Authorize(Roles = "Admin")]
    [HttpPost("crowd/deposit-offer")]
    public async Task<IActionResult> CreateCrowdDepositOffer([FromBody] CreateCrowdDepositOfferDto createCrowdDepositOfferDto)
    {
        return Ok(await adminService.CreateCrowdDepositOfferAsync(createCrowdDepositOfferDto));
    }

    /// <summary>
    /// Update an existing crowd deposit offer
    /// </summary>
    /// <param name="updateUserRequestDto">Updated offer details</param>
    /// <returns>Updated crowd deposit offer</returns>
    /// <response code="200">Crowd deposit offer updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have admin privileges</response>
    /// <response code="404">Offer not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("crowd/deposit-offer")]
    public async Task<IActionResult> UpdateCrowdDepositOffer([FromBody] UpdateCrowdDepositOfferDto updateUserRequestDto)
    {
        return Ok(await adminService.UpdateCrowdDepositOfferAsync(updateUserRequestDto));
    }

    /// <summary>
    /// Delete a crowd deposit offer
    /// </summary>
    /// <param name="deleteCrowdDepositOfferDto">Delete request details</param>
    /// <returns>Confirmation of deletion</returns>
    /// <response code="200">Crowd deposit offer deleted successfully</response>
    /// <response code="400">Invalid delete request</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have admin privileges</response>
    /// <response code="404">Offer not found</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("crowd/deposit-offer")]
    public async Task<IActionResult> DeleteCrowdDepositOffer([FromBody] DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto)
    {
        return Ok(await adminService.DeleteCrowdDepositOfferAsync(deleteCrowdDepositOfferDto));
    }

    /// <summary>
    /// Approve a custom deposit request
    /// </summary>
    /// <param name="approveCustomDepositRequestDto">Approval details</param>
    /// <returns>Confirmation of approval</returns>
    /// <response code="200">Custom deposit request approved successfully</response>
    /// <response code="400">Invalid approval request</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have admin privileges</response>
    /// <response code="404">Custom deposit request not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("custom/approve")]
    public async Task<IActionResult> ApproveCrowdDepositOffer(ApproveCustomDepositRequestDto approveCustomDepositRequestDto)
    {
        return Ok(await adminService.ApproveCustomDepositAsync(approveCustomDepositRequestDto));
    }
    
    /// <summary>
    /// Reject a custom deposit request
    /// </summary>
    /// <param name="rejectCustomDepositOfferRequestDto">Rejection details</param>
    /// <returns>Confirmation of rejection</returns>
    /// <response code="200">Custom deposit request rejected successfully</response>
    /// <response code="400">Invalid rejection request</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have admin privileges</response>
    /// <response code="404">Custom deposit request not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("custom/reject")]
    public async Task<IActionResult> RejectCrowdDepositOffer(RejectCustomDepositRequestDto rejectCustomDepositOfferRequestDto)
    {
        return Ok(await adminService.RejectCustomDepositAsync(rejectCustomDepositOfferRequestDto));
    }
}