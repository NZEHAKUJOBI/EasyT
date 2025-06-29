namespace backend.Api.Entities;

public class Driver
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime LicenseExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? PhoneNumber { get; set; } = null;
    public bool IsActive { get; set; } = true; // Indicates if the driver account is active
    public DateTime? UpdatedAt { get; set; } = null; // Timestamp for the last update to the driver profile
    public string PhotoUrl { get; set; } = string.Empty; // URL to the driver's photo
    public bool isOnline { get; set; } = false; // Indicates if the driver is currently online
    public bool isverified { get; set; } = false; // Indicates if the driver's account is verified
    public double DriverLatitude { get; set; } = 0.0; // Current latitude of the driver
    public double DriverLongitude { get; set; } = 0.0; // Current longitude of the driver
    }