using System;
using System.Threading.Tasks;
using API.Data;
using backend.API.Entities;
using backend.API.DTO.Request;
using Microsoft.Extensions.Logging;

namespace backend.API.Services
{
    public class UserLocationService
    {
        private readonly ILogger<UserLocationService> _logger;
        private readonly AppDbContext _context;

        public UserLocationService(ILogger<UserLocationService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<UserLocationRequestDto?> GetUserLocationAsync(Guid userId, double latitude, double longitude)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogError("Invalid UserId provided.");
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            }

            try
            {
                var userLocation = await _context.UserLocations.FindAsync(userId);
                if (userLocation == null)
                {
                    _logger.LogInformation("User location not found for UserId: {UserId}", userId);
                    return null;
                }

                return new UserLocationRequestDto
                {
                    UserId = userLocation.UserId,
                    Latitude = userLocation.Latitude,
                    Longitude = userLocation.Longitude,
                    LastUpdated = userLocation.LastUpdated
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user location for UserId: {UserId}", userId);
                throw;
            }
        }
    }
}
