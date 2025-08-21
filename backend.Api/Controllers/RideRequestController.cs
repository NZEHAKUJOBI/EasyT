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

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Restrict access to only users with the Passenger role
    public class RideRequestController : ControllerBase
    {
        private readonly ILogger<RideRequestController> _logger;
        private readonly RideRequestService _rideRequestService;

        public RideRequestController(ILogger<RideRequestController> logger, RideRequestService rideRequestService)
        {
            _logger = logger;
            _rideRequestService = rideRequestService;
        }

        [HttpPost("request")]
        [SwaggerOperation(Summary = "Request a ride", Description = "Allows a passenger to request a ride by providing pickup and dropoff locations.")]
        public async Task<ActionResult<ServiceResponseDto<string>>> RequestRide(
            [FromBody] RideRequestDto dto, 
            CancellationToken cancellationToken)
        {   
            var PassengerIdStr = User.FindFirst("UserId")?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResponseDto<string>.FailResponse("Invalid input data."));
            }

            if (!Guid.TryParse(PassengerIdStr, out Guid PassengerId))
            {
                return BadRequest(ServiceResponseDto<string>.FailResponse("Invalid PassengerId."));
            }

            try
            {
                var result = await _rideRequestService.CreateRideRequestAsync(dto, PassengerId, cancellationToken);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating ride request");
                return StatusCode(500, ServiceResponseDto<string>.FailResponse("An error occurred while requesting the ride."));
            }
        }
    }
}
