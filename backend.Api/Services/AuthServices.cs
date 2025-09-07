using API.Data;
using backend.Api.DTO.Response;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using API.Interface;
using backend.API.DTO.Request;



namespace API.Services
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
    var tenant = await _context.Tenants.SingleOrDefaultAsync(t => t.Email == dto.Email, cancellationToken);

    if (user == null && tenant == null)
        return ServiceResponseDto<string>.FailResponse("No user or tenant found with this email.");

    if (_otpService.ValidateOtp(dto.Email, dto.OtpCode))
    {
        if (user != null)
            user.IsEmailVerified = true;

        if (tenant != null)
        {
            tenant.IsEmailVerified = true;
            tenant.IsActive = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return ServiceResponseDto<string>.SuccessResponse("Email verified successfully.");
    }

    return ServiceResponseDto<string>.FailResponse("Invalid OTP.");
}


        public async Task<ServiceResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
            if (user == null)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Invalid email or password.");

            if (!user.IsEmailVerified)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Email not verified.");
            var IsEmailVerified = user.IsEmailVerified;
            if (!IsEmailVerified)
                return ServiceResponseDto<LoginResponseDto>.FailResponse("Email not verified. Please verify your email before logging in.");
            

            var token = _jwtService.GenerateToken(user);
            return ServiceResponseDto<LoginResponseDto>.SuccessResponse(new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName} {user.MiddleName}".Trim(),
                Email = user.Email,
                Role = user.Role
            });
        }
          public async Task<ServiceResponseDto<string>> RegisterTenant(RegisterTenantRequestDto dto)
{
    // Check if email already exists in tenants or users
    if (await EmailExists(dto.Email))
        return Fail("Email already exists for a tenant or user.");

    // Validate password
    if (!PasswordValidatorUtility.IsValidPassword(dto.AdminPassword))
        return Fail("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

    var driverId = Guid.NewGuid();
    var hashedPassword = _passwordHasher.HashPassword(null, dto.AdminPassword);

    // Create tenant entity
    var tenant = new Tenant
    {
        Id = driverId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        MiddleName = dto.MiddleName,
        DriverId = driverId,
        AdminPassword = hashedPassword,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        BaseFare = 500,
        PerKilometerRate = 100,
        CommissionRate = 0.10m,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        IsActive = false,
        Role = "Driver"
    };

    // Create tenant admin user
    var adminUser = new User
    {
        Id = driverId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        MiddleName = dto.MiddleName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        Role = "Driver",
        IsEmailVerified = false,
        PasswordHash = _passwordHasher.HashPassword(null, dto.AdminPassword)
    };

    // Save to DB
    await _context.Tenants.AddAsync(tenant);
    await _context.Users.AddAsync(adminUser);
    await _context.SaveChangesAsync();

    // Send OTP
    var otp = _otpService.GenerateOtp();
    _otpService.StoreOtp(dto.Email, otp);
    await _emailService.SendEmailAsync(dto.Email, "Verify your email", $"Your OTP is: {otp}");

    return Success("Tenant registered successfully.");
}

private async Task<bool> EmailExists(string email)
{
    return await _context.Tenants.AnyAsync(t => t.Email == email) ||
           await _context.Users.AnyAsync(u => u.Email == email);
}

private static ServiceResponseDto<string> Fail(string message) =>
    ServiceResponseDto<string>.FailResponse(message);

private static ServiceResponseDto<string> Success(string message) =>
    ServiceResponseDto<string>.SuccessResponse(message);

    } }


        