namespace API.Entities;

public enum Status
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}

public class VehicleEntity
{
    public Guid Id { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty; // e.g., Bus, Car, Van
    public double PickupLatitude { get; set; } = 0.0; // Current latitude of the vehicle
    public double PickupLongitude { get; set; } = 0.0; // Current longitude of the vehicle
    public double DropupLatitude { get; set; } = 0.0; // Drop-off latitude of the vehicle
    public double DropupLongitude { get; set; } = 0.0; // Drop-off longitude of the vehicle
    public double DistanceCovered { get; set; } = 0.0; // Total distance covered by the vehicle
    public double FareEstimate { get; set; } = 0.0; // Estimated fare for the trip
    public double FareFinal { get; set; } = 0.0; // Final fare after the trip is completed
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow; // Timestamp when the vehicle was requested
    public DateTime StartedAt { get; set; } = DateTime.UtcNow; // Timestamp when the trip started
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow; // Timestamp when the trip was completed
    public DateTime CancelledAt { get; set; } = DateTime.UtcNow; // Timestamp when the trip was cancelled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Pending; // Current status of the vehicle
}