
// Update the using directive to the correct namespace for your DTOs
using backend.Api.DTO.Response;
using backend.API.DTO.Request;

namespace API.Services
{
    public interface IAuthService
    {
        Task<ServiceResponseDto<string>> RegisterAsync(RegisterUserDto dto);

        Task<ServiceResponseDto<string>> VerifyEmailAsync(EmailOtpDto dto, CancellationToken cancellationToken = default);

        Task<ServiceResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    }
}
