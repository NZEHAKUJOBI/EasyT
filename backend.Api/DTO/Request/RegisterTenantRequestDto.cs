using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace backend.API.DTO.Request;

public class RegisterTenantRequestDto
{

    public Guid DriverId { get; set; }
    public string FirstName { get; set; }

    public string? MiddleName { get; set; } = null;
    public string LastName { get; set; }

     public string AdminPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
   



    


}