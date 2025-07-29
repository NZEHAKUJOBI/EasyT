// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using backend.API.DTO.Request;
// using backend.API.DTO.Response;
// using backend.API.Entities;
// using API.Data;
// using Microsoft.EntityFrameworkCore;
// using backend.API.Services;
// using System.Security.Claims;

// namespace backend.API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     [Authorize]
//     public class BusDriverController : ControllerBase
//     {
//         private readonly AppDbContext _context;
//         private readonly ILogger<BusDriverController> _logger;
        

//         public BusDriverController(AppDbContext context, ILogger<BusDriverController> logger)
//         {
//             _context = context;
//             _logger = logger;
          
//         }

//         [HttpPost("register")]
//         [Authorize(Roles = "Admin")]
//         public async Task<IActionResult> RegisterBusDriver([FromBody] RegisterDriverDto dto, CancellationToken cancellationToken)
//         {
//             if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.TenantId == _tenantProvider.TenantId, cancellationToken))
//             {
//                 return BadRequest(ServiceResponseDto<string>.FailResponse("Driver with this email already exists."));
//             }

//             var driver = new User
//             {
//                 FirstName = dto.FirstName,
//                 LastName = dto.LastName,
//                 Email = dto.Email,
//                 PhoneNumber = dto.PhoneNumber,
//                 Role = "BusDriver",
//                 TenantId = _tenantProvider.TenantId,
//                 PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
//             };

//             await _context.Users.AddAsync(driver, cancellationToken);
//             await _context.SaveChangesAsync(cancellationToken);

//             _logger.LogInformation("Bus driver {Email} registered under tenant {TenantId}", driver.Email, driver.TenantId);

//             return Ok(ServiceResponseDto<string>.SuccessResponse("Bus driver registered successfully."));
//         }

//         [HttpGet("profile")]
//         [Authorize(Roles = "BusDriver")]
//         public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
//         {
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             if (!Guid.TryParse(userId, out var guidUserId))
//                 return Unauthorized(ServiceResponseDto<string>.FailResponse("Invalid user context."));

//             var driver = await _context.Users
//                 .FirstOrDefaultAsync(u => u.Id == guidUserId && u.Role == "BusDriver" && u.TenantId == _tenantProvider.TenantId, cancellationToken);

//             if (driver == null)
//                 return NotFound(ServiceResponseDto<string>.FailResponse("Bus driver not found."));

//             var profile = new DriverProfileDto
//             {
//                 Id = driver.Id,
//                 FirstName = driver.FirstName,
//                 LastName = driver.LastName,
//                 Email = driver.Email,
//                 PhoneNumber = driver.PhoneNumber
//             };

//             return Ok(ServiceResponseDto<DriverProfileDto>.SuccessResponse(profile));
//         }

//         [HttpPut("profile")]
//         [Authorize(Roles = "BusDriver")]
//         public async Task<IActionResult> UpdateProfile([FromBody] UpdateDriverProfileDto dto, CancellationToken cancellationToken)
//         {
//             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//             if (!Guid.TryParse(userId, out var guidUserId))
//                 return Unauthorized(ServiceResponseDto<string>.FailResponse("Invalid user context."));

//             var driver = await _context.Users
//                 .FirstOrDefaultAsync(u => u.Id == guidUserId && u.Role == "BusDriver" && u.TenantId == _tenantProvider.TenantId, cancellationToken);

//             if (driver == null)
//                 return NotFound(ServiceResponseDto<string>.FailResponse("Bus driver not found."));

//             driver.FirstName = dto.FirstName ?? driver.FirstName;
//             driver.LastName = dto.LastName ?? driver.LastName;
//             driver.PhoneNumber = dto.PhoneNumber ?? driver.PhoneNumber;

//             _context.Users.Update(driver);
//             await _context.SaveChangesAsync(cancellationToken);

//             _logger.LogInformation("Bus driver {Email} updated their profile.", driver.Email);

//             return Ok(ServiceResponseDto<string>.SuccessResponse("Profile updated successfully."));
//         }
//     }
// }
