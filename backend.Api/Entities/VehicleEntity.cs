namespace API.Entities;

public class VehicleEntityV2
{
    public Guid Id { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty; // e.g., Bus, Car, Van
    public string Model { get; set; } = string.Empty;
    public int Capacity { get; set; } // Number of passengers the vehicle can carry
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null; // Timestamp for the last update to the vehicle profile
    public bool IsActive { get; set; } = true; // Indicates if the vehicle is currently active
    public string? PhotoUrl { get; set; } = null; // URL to the vehicle's photo
    public Guid DriverId { get; set; } // Foreign key to the driver entity
}