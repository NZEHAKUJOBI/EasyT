using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.API.DTO.Request;

public class RegisterTenantRequestDto
{

     [JsonIgnore] // hides from JSON serialization
    [SwaggerSchema(ReadOnly = true, WriteOnly = true)] 
    public Guid DriverId { get; set; } = new Guid();
    public string FirstName { get; set; }

    public string? MiddleName { get; set; } = null;
    public string LastName { get; set; }

     public string AdminPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
   



    


}