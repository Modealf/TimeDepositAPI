using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                await _authService.RegisterAsync(request);
                var loginRequest = new LoginRequestDto { Email = request.Email, Password = request.Password };
                var token = await _authService.LoginAsync(loginRequest);
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
                var token = await _authService.LoginAsync(request);
                return Ok(new LoginResponseDto { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
