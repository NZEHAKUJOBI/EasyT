using API.Data;
using API.DTO.Response;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Interface;


namespace AuthExample.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public AuthService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService,
            IEmailService emailService,
            IOtpService otpService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _emailService = emailService;
            _otpService = otpService;
        }

        public async Task<ServiceResponseDto<string>> RegisterAsync(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return ServiceResponseDto<string>.FailResponse("Username is already taken.");
        
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return ServiceResponseDto<string>.FailResponse("Email is already in use.");
        
            var user = new User
            {
                Email = dto.Email, // Assuming username is the email
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                PhoneNumber = dto.PhoneNumber,
                Role = "Passenger", // Default role
                Id = Guid.NewGuid(),
                 
            };
        
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        
            var otp = _otpService.GenerateOtp();
            _otpService.StoreOtp(user.Email, otp);
        
            await _emailService.SendEmailAsync(user.Email, "Verify your email", $"Your OTP is: {otp}");
        
            return ServiceResponseDto<string>.SuccessResponse("User registered. Please check your email for OTP to verify your account.");
        }

        public async Task<ServiceResponseDto<string>> VerifyEmailAsync(EmailOtpDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
            if (user == null)
                return ServiceResponseDto<string>.FailResponse("User not found.");

            if (_otpService.ValidateOtp(dto.Email, dto.OtpCode))
            {
                user.IsEmailVerified = true;
                await _context.SaveChangesAsync(cancellationToken);
                return ServiceResponseDto<string>.SuccessResponse("Email verified successfully.");
            }

            return ServiceResponseDto<string>.FailResponse("Invalid OTP.");
        }

        public async Task<ServiceResponseDto<LoginResponseDto>> LoginAsync(LoginResponseDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
            if (user == null)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Invalid username or password.");

            if (!user.IsEmailVerified)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Please verify your email before logging in.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.password);
            if (result == PasswordVerificationResult.Failed)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Invalid username or password.");

            var token = _jwtService.GenerateToken(user);

            var responseDto = new LoginResponseDto { Token = token };

            return ServiceResponseDto<LoginResponseDto>.SuccessResponse(responseDto, "Login successful.");
        }
    }
}
