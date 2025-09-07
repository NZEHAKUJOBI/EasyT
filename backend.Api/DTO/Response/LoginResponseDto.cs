namespace backend.Api.DTO.Response
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Passenger"; // Default role is Passenger

        public string password { get; set; } = string.Empty; // Password is not used in the response, but included for consistency
    }
   
}