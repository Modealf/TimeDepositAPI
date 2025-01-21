using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService adminService) : ControllerBase
{

    [HttpPost("deposit-offer")]
    public async Task<IActionResult> CreateDepositOffer([FromBody] CreateCrowdDepositOfferDto createCrowdDepositOfferDto)
    {
        return Ok(await adminService.CreateCrowdDepositOfferAsync(createCrowdDepositOfferDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("deposit-offer")]
    public async Task<IActionResult> UpdateDepositOffer([FromBody] UpdateCrowdDepositOfferDto updateUserRequestDto)
    {
        return Ok(await adminService.UpdateCrowdDepositOfferAsync(updateUserRequestDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deposit-offer")]
    public async Task<IActionResult> DeleteDepositOffer([FromBody] DeleteCrowdDepositOfferDto deleteCrowdDepositOfferDto)
    {
        return Ok(await adminService.DeleteCrowdDepositOfferAsync(deleteCrowdDepositOfferDto));
    }
}