using System;
using System.Threading.Tasks;
using API.Data;
using backend.API.Entities;
using backend.API.DTO.Request;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using backend.Api.DTO.Response;
using API.Entities;
using API.Interface;
namespace API.Services
{
    public class DriverService : IDriverService
    {
        private readonly ILogger<RideRequestService> _logger;
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DriverService(ILogger<RideRequestService> logger, AppDbContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<ServiceResponseDto<string>> UpdateVehicleLocation(Guid driverId, double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            // Validate Driver
            var driver = await _context.Tenants
                .FirstOrDefaultAsync(u => u.Id == driverId && u.Role == "Driver", cancellationToken);
            if (driver == null)
                return ServiceResponseDto<string>.FailResponse("Driver not found.");

            try
            {
                _logger.LogInformation($"Updating vehicle location for driver {driver.Email} with ID: {driverId}");

                // Update driver's vehicle location
                if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
                {
                    _logger.LogWarning($"Invalid latitude or longitude values: {latitude}, {longitude}");
                    return ServiceResponseDto<string>.FailResponse("Invalid latitude or longitude values.");
                }
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var busDriver = await dbContext.Tenants
                    .FirstOrDefaultAsync(b => b.Id == driverId && b.Role == "Driver");

                if (busDriver == null)
                    return ServiceResponseDto<string>.FailResponse("Driver not found.");
                if (!busDriver.IsActive)
                    return ServiceResponseDto<string>.FailResponse("Driver is not active.");
                var lastTimestamp = await dbContext.BusLocations
                   .Where(b => b.DiverId == driverId)
                   .OrderByDescending(b => b.Timestamp)
                   .Select(b => b.Timestamp)
                   .FirstOrDefaultAsync(cancellationToken);
                var minInterval = TimeSpan.FromSeconds(20); // Minimum interval of 30 seconds  
                if (lastTimestamp != default && DateTime.UtcNow - lastTimestamp < minInterval)
                    return ServiceResponseDto<string>.FailResponse("Location update too frequent. Please wait before updating again.");

                var lastLocation = await dbContext.BusLocations
                    .Where(b => b.DiverId == driverId)
                    .OrderByDescending(b => b.Timestamp)
                    .FirstOrDefaultAsync(cancellationToken);
                if (lastLocation != null && Math.Abs(lastLocation.Latitude - latitude) < 0.0001 && Math.Abs(lastLocation.Longitude - longitude) < 0.0001)
                {
                    _logger.LogInformation("No significant change in location. Skipping update.");
                    return ServiceResponseDto<string>.SuccessResponse("No significant change in location.");
                }

                var driverName = await dbContext.Tenants
                    .Where(b => b.Id == driverId)
                    .Select(b => $"{b.FirstName} {b.LastName}")
                    .FirstOrDefaultAsync(cancellationToken);

                var busLocation = new BusLocation
                {
                    DiverId = driverId,
                    TripId = Guid.NewGuid(), // Assuming a new trip for each location update
                    Latitude = latitude,
                    Longitude = longitude,
                    Timestamp = DateTime.UtcNow,
                    DriverName = driverName ?? "Unknown Driver"

                };

                await dbContext.BusLocations.AddAsync(busLocation, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                return ServiceResponseDto<string>.SuccessResponse("Vehicle location updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle location for driver {DriverId}", driverId);
                return ServiceResponseDto<string>.FailResponse("An error occurred while updating vehicle location.");
            }
        }


    }

}