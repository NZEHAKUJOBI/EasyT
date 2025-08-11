using System;
using System.Threading.Tasks;
using API.Data;
using backend.API.Entities;
using backend.API.DTO.Request;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using backend.Api.DTO.Response;
using API.Entities;
namespace API.Services
{
    public class RideRequestService
    {
        private readonly ILogger<RideRequestService> _logger;
        private readonly AppDbContext _context;

        public RideRequestService(ILogger<RideRequestService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ServiceResponseDto<string>> CreateRideRequestAsync(RideRequestDto dto, CancellationToken cancellationToken = default)
        {

            // 2 Validate Passenger
            var passenger = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.PassengerId && u.Role == "Passenger", cancellationToken);
            if (passenger == null)
                return ServiceResponseDto<string>.FailResponse("Passenger not found.");


            // 4️Create RideRequest entity
            var rideRequest = new RideRequest
            {
                PassengerId = passenger.Id,
                PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                PickupLocation = dto.PickupLocation,
                PickupLatitude = dto.PickupLatitude,
                PickupLongitude = dto.PickupLongitude,
                DropoffLocation = dto.DropoffLocation,
                DropoffLatitude = dto.DropoffLatitude,
                DropoffLongitude = dto.DropoffLongitude,
                Status = RideStatus.Requested, // Default status
            };

            await _context.RideRequests.AddAsync(rideRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Ride requested by passenger {passenger.Email} with RideId: {rideRequest.PassengerId}");

            return ServiceResponseDto<string>.SuccessResponse($"Ride requested successfully with ID: {rideRequest.PassengerId}");
        }

        /// <summary>
        /// Basic Haversine formula for distance calculation in KM.
        /// </summary>
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the earth in km
            var latRad = ToRadians(lat2 - lat1);
            var lonRad = ToRadians(lon2 - lon1);
            var a = Math.Sin(latRad / 2) * Math.Sin(latRad / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(lonRad / 2) * Math.Sin(lonRad / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double deg) => deg * (Math.PI / 180);
    
    
    public async Task<ServiceResponseDto<RideRequest>> GetRideRequestByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var rideRequest = await _context.RideRequests
            .Include(r => r.Passenger)
            .Include(r => r.Driver)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return rideRequest is null
            ? ServiceResponseDto<RideRequest>.FailResponse("Ride request not found.")
            : ServiceResponseDto<RideRequest>.SuccessResponse(rideRequest);
    }

}

}
