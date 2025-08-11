using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using backend.API.DTO.Request;
using backend.Api.DTO.Response;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
// Add the correct namespace for IDriverService below if it's different
using backend.API.Services;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Restrict access to only users with the Passenger role
    public class BusDriverController : ControllerBase
    {
        private readonly ILogger<RideRequestController> _logger;
        private readonly RideRequestService _rideRequestService;
        private readonly DriverService _driver;

        public BusDriverController(ILogger<RideRequestController> logger, RideRequestService rideRequestService, DriverService driver)
        {
            _logger = logger;
            _rideRequestService = rideRequestService;
            _driver = driver;
        }

        [HttpPost("UpdateVehicleLocation")]
        [SwaggerOperation(Summary = "Update vehicle location", Description = "Allows a driver to update their vehicle's current location.")]
        public async Task<ActionResult<ServiceResponseDto<string>>> UpdateVehicleLocation(
            [FromBody] RideRequestDto dto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResponseDto<string>.FailResponse("Invalid input data."));
            }

            try
            {
                var result = await _driver.UpdateVehicleLocation(dto.PassengerId, dto.PickupLatitude, dto.PickupLongitude, cancellationToken);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating vehicle location");
                return StatusCode(500, ServiceResponseDto<string>.FailResponse("An error occurred while updating the vehicle location."));
            }
        }


    }
}
