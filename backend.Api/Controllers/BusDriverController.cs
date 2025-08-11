using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using backend.API.DTO.Request;
using backend.Api.DTO.Response;
using API.Interface;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Driver")]
    public class BusDriverController : ControllerBase
    {
        private readonly ILogger<BusDriverController> _logger;
        private readonly IDriverService _driverService;

        public BusDriverController(ILogger<BusDriverController> logger, IDriverService driverService)
        {
            _logger = logger;
            _driverService = driverService;
        }

        [HttpPost("UpdateVehicleLocation")]
        [SwaggerOperation(Summary = "Update vehicle location", Description = "Allows a driver to update their vehicle's current location.")]
        [ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponseDto<string>>> UpdateVehicleLocation(
            [FromBody] RideRequestDto dto,
            CancellationToken cancellationToken)
        {
            var driverIdClaim = User.FindFirst("DriverId")?.Value;
            if (!Guid.TryParse(driverIdClaim, out var driverId))
                return Unauthorized(ServiceResponseDto<string>.FailResponse("Driver ID missing or invalid in token."));

            if (dto is null || !ModelState.IsValid)
                return BadRequest(ServiceResponseDto<string>.FailResponse("Invalid input data."));

            try
            {
                var result = await _driverService.UpdateVehicleLocation(driverId, dto.PickupLatitude, dto.PickupLongitude, cancellationToken);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("UpdateVehicleLocation cancelled for DriverId {DriverId}.", driverId);
                return StatusCode(StatusCodes.Status499ClientClosedRequest,
                    ServiceResponseDto<string>.FailResponse("Request was cancelled."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating vehicle location for DriverId {DriverId}.", driverId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ServiceResponseDto<string>.FailResponse("An unexpected error occurred."));
            }
        }
    }
}
