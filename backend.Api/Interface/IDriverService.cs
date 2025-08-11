

using backend.Api.DTO.Response;
using backend.API.DTO.Request;
namespace API.Interface

{

    public interface IDriverService
    {
        Task<ServiceResponseDto<string>>UpdateVehicleLocation(Guid driverId, double latitude, double longitude, CancellationToken cancellationToken = default);
        
    }
}