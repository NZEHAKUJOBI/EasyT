using Swashbuckle.AspNetCore.Annotations;

namespace API.Entities;

public class User
{
    [SwaggerSchema(ReadOnly = true)]


    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Passenger";
    public string? PhoneNumber { get; set; } = null;

    public bool IsEmailVerified { get; set; } = false; // Default to false
}
