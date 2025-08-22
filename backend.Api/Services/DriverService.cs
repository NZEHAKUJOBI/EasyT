using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interface;
using backend.Api.DTO.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace API.Services
{
    public class DriverService : IDriverService
    {
        private readonly ILogger<DriverService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DriverService(ILogger<DriverService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

       public async Task<ServiceResponseDto<string>> UpdateVehicleLocation(
    Guid driverId,
    double latitude,
    double longitude,
    CancellationToken cancellationToken = default)
{
    if (!IsValidCoordinates(latitude, longitude))
    {
        _logger.LogWarning("Invalid latitude/longitude: {Lat}, {Lng}", latitude, longitude);
        return Fail("Invalid latitude or longitude values.");
    }

    try
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var driver = await dbContext.Tenants
            .FirstOrDefaultAsync(d => d.Id == driverId && d.Role == "Driver", cancellationToken);

        if (driver == null)
            return Fail("Driver not found.");

        if (!driver.IsActive)
            return Fail("Driver is not active.");

        if (await IsTooFrequentUpdate(dbContext, driverId, cancellationToken))
            return Fail("Location update too frequent. Please wait before updating again.");

        if (await IsNoSignificantChange(dbContext, driverId, latitude, longitude, cancellationToken))
        {
            _logger.LogInformation("No significant change in location. Skipping update.");
            return Success("No significant change in location.");
        }

        var busLocation = new BusLocation
        {
            Id = Guid.NewGuid(),
            DriverId = driverId,
            TripId = Guid.NewGuid(), // consider passing this if you want trip grouping
            Latitude = latitude,
            Longitude = longitude,
            Timestamp = DateTime.UtcNow,
            Status = "Active",
            DriverName = $"{driver.FirstName} {driver.LastName}"
        };

        await dbContext.BusLocations.AddAsync(busLocation, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success("Vehicle location updated successfully.");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating vehicle location for driver {DriverId}", driverId);
        return Fail("An error occurred while updating vehicle location.");
    }
}


        #region Helpers

        private static bool IsValidCoordinates(double latitude, double longitude) =>
            latitude is >= -90 and <= 90 && longitude is >= -180 and <= 180;

        private static ServiceResponseDto<string> Fail(string message) =>
            ServiceResponseDto<string>.FailResponse(message);

        private static ServiceResponseDto<string> Success(string message) =>
            ServiceResponseDto<string>.SuccessResponse(message);

        private async Task<bool> IsTooFrequentUpdate(AppDbContext dbContext, Guid driverId, CancellationToken ct)
        {
            var lastTimestamp = await dbContext.BusLocations
                .Where(b => b.DriverId == driverId)
                .OrderByDescending(b => b.Timestamp)
                .Select(b => b.Timestamp)
                .FirstOrDefaultAsync(ct);

            var minInterval = TimeSpan.FromSeconds(20);
            return lastTimestamp != default && DateTime.UtcNow - lastTimestamp < minInterval;
        }

        private async Task<bool> IsNoSignificantChange(AppDbContext dbContext, Guid driverId, double lat, double lng, CancellationToken ct)
        {
            var lastLocation = await dbContext.BusLocations
                .Where(b => b.DriverId == driverId)
                .OrderByDescending(b => b.Timestamp)
                .FirstOrDefaultAsync(ct);

            return lastLocation != null &&
                   Math.Abs(lastLocation.Latitude - lat) < 0.0001 &&
                   Math.Abs(lastLocation.Longitude - lng) < 0.0001;
        }

        #endregion
    }
}
