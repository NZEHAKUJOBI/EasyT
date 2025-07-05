
using API.DTO.Response;

namespace API.Services
{
    public interface IAuthService
    {
        Task<ServiceResponseDto<string>> RegisterAsync(RegisterUserDto dto);

        Task<ServiceResponseDto<string>> VerifyEmailAsync(EmailOtpDto dto, CancellationToken cancellationToken = default);

        Task<ServiceResponseDto<LoginResponseDto>> LoginAsync(LoginResponseDto dto, CancellationToken cancellationToken = default);
    }
}
