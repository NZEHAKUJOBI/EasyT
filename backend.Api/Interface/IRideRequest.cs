

using backend.Api.DTO.Response;
using backend.API.DTO.Request;
namespace API.Interface

{

    public interface IRideRequest
    {
        Task<ServiceResponseDto<string>> CreateRideRequestAsync(RideRequestDto dto, Guid PassengerId,CancellationToken cancellationToken = default);
    }
}