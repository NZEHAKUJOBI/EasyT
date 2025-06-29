namespace backend.Api.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Passenger"; // Admin, Driver, Passenger
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfilePictureUrl { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    public bool IsActive { get; set; } = true; // Indicates if the user account is active
    public DateTime? UpdatedAt { get; set; } = null; // Timestamp for the last update to the user profile

}
