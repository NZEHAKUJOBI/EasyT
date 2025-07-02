namespace API.DTO.Response;

public class RegisterUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Passenger"; // Default role is Passenger

    public string Password { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = null; // Optional phone number

    public bool IsEmailVerified { get; set; } = false; // Default to false
  
   
}