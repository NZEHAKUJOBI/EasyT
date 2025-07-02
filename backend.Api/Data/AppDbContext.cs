using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Services;

namespace  API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<BusLocation> BusLocations => Set<BusLocation>();
}
