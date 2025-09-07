using Microsoft.AspNetCore.Mvc;
using backend.API.DTO.Request;
using backend.Api.DTO.Response;
using API.Interface;
using API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">User registration data.</param>
        /// <returns>Registration result.</returns>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user account.")]
        [SwaggerResponse(200, "User registered successfully")]
        [SwaggerResponse(400, "Registration failed")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Verifies user's email using OTP.
        /// </summary>
        /// <param name="dto">Email OTP data.</param>
        /// <returns>Email verification result.</returns>
        [HttpPost("verify-email")]
        [SwaggerOperation(Summary = "Verify email with OTP", Description = "Verifies user's email address using OTP.")]
        [SwaggerResponse(200, "Email verified successfully")]
        [SwaggerResponse(400, "Verification failed")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailOtpDto dto)
        {
            var result = await _authService.VerifyEmailAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="dto">Login data.</param>
        /// <returns>Login result.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Authenticates user and returns login result.")]
        [SwaggerResponse(200, "Login successful")]
        [SwaggerResponse(400, "Login failed")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

 [HttpPost("register-tenant")]
[SwaggerOperation(Summary = "Register a tenant", Description = "Creates a new tenant account with admin credentials.")]
[ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status400BadRequest)]
public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantRequestDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ServiceResponseDto<string>.FailResponse("Invalid input data."));

    var result = await _authService.RegisterTenant(dto);
    return result.Success ? Ok(result) : BadRequest(result);
}

    }
}
