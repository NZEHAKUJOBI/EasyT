using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Services;
using backend.API.Entities;

namespace  API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<BusLocation> BusLocations => Set<BusLocation>();
    public DbSet<PaymentHistory> PaymentHistories => Set<PaymentHistory>();
    public DbSet<UserLocation> UserLocations => Set<UserLocation>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<RideRequest> RideRequests => Set<RideRequest>();
    public DbSet<GeneralNotification> GeneralNotifications { get; set; }

}
