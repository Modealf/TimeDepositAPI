using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                await authService.RegisterAsync(request);
                var loginRequest = new LoginRequestDto { Email = request.Email, Password = request.Password };
                var token = await authService.LoginAsync(loginRequest);
                return Ok(new LoginResponseDto { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var token = await authService.LoginAsync(request);
                return Ok(new RegisterResponseDto { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserRequestDto request)
        {
            try
            {
                var message = await authService.UpdateUserInfoAsync(request);
                return Ok(new UpdateUserResponseDto { Message = message });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
