using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Controllers
{
    /// <summary>
    /// Controller for handling user authentication operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="request">User registration details</param>
        /// <returns>JWT token for the newly registered user</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid registration data or user already exists</response>
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

        /// <summary>
        /// Authenticate user and obtain JWT token
        /// </summary>
        /// <param name="request">User login credentials</param>
        /// <returns>JWT token for authentication</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid credentials</response>
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

        /// <summary>
        /// Update authenticated user's information
        /// </summary>
        /// <param name="request">Updated user information</param>
        /// <returns>Confirmation message</returns>
        /// <response code="200">User information updated successfully</response>
        /// <response code="400">Invalid update data</response>
        /// <response code="401">User not authenticated</response>
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
