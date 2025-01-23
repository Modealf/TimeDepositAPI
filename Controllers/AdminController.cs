using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService adminService, IBackgroundJobClient backgroundJobClient) : ControllerBase
{
        
    [Authorize(Roles = "Admin")]
    [HttpPost("crowd/deposit-offer")]
    public async Task<IActionResult> CreateCrowdDepositOffer([FromBody] CreateCrowdDepositOfferDto createCrowdDepositOfferDto)
    {
        return Ok(await adminService.CreateCrowdDepositOfferAsync(createCrowdDepositOfferDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("crowd/deposit-offer")]
    public async Task<IActionResult> UpdateCrowdDepositOffer([FromBody] UpdateCrowdDepositOfferDto updateUserRequestDto)
    {
        return Ok(await adminService.UpdateCrowdDepositOfferAsync(updateUserRequestDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("crowd/deposit-offer")]
    public async Task<IActionResult> DeleteCrowdDepositOffer([FromBody] DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto)
    {
        return Ok(await adminService.DeleteCrowdDepositOfferAsync(deleteCrowdDepositOfferDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("custom/approve")]
    public async Task<IActionResult> ApproveCrowdDepositOffer(ApproveCustomDepositRequestDto approveCustomDepositRequestDto)
    {
        return Ok(await adminService.ApproveCustomDepositAsync(approveCustomDepositRequestDto));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("custom/reject")]
    public async Task<IActionResult> RejectCrowdDepositOffer(RejectCustomDepositRequestDto rejectCustomDepositOfferRequestDto)
    {
        return Ok(await adminService.RejectCustomDepositAsync(rejectCustomDepositOfferRequestDto));
    }
}