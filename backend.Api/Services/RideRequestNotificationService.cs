using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class RideRequestNotificationService : BackgroundService
    {
        private readonly ILogger<RideRequestNotificationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RideRequestNotificationService(
            ILogger<RideRequestNotificationService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RideRequestNotificationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // 1. Fetch pending ride requests
                    var rideRequests = await dbContext.RideRequests
                        .Where(r => !r.IsAssigned) // adjust to your RideRequest schema
                        .ToListAsync(stoppingToken);

                    foreach (var request in rideRequests)
                    {
                        // 2. Find nearby drivers (simplified example: within 2km)
                        var nearbyDrivers = await dbContext.BusLocations
                            .Where(b => b.Status == "Active" &&
                                        Distance(b.Latitude, b.Longitude,
                                                 request.PickupLat, request.PickupLng) < 2.0)
                            .ToListAsync(stoppingToken);

                        foreach (var driver in nearbyDrivers)
                        {
                            var notification = new GeneralNotification
                            {
                                DriverId = driver.DriverId,
                                Message = $"New ride request at {request.PickupAddress}",
                                Type = "RideRequest"
                            };

                            await dbContext.GeneralNotifications.AddAsync(notification, stoppingToken);
                        }

                        // Optional: Mark request as "notified"
                        request.IsNotified = true;
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing ride requests in RideRequestNotificationService.");
                }

                // Run every 20 seconds
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }

        // Haversine distance (km)
        private static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // km
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
