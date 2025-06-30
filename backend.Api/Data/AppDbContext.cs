using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Trip> Trips { get; set; }
  
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<VehicleEntityV2> VehicleEntities { get; set; }
    public DbSet<Payment> Payments { get; set; }

}
