using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Interface;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// ----------------- Configure Services ------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped< RideRequestService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddLogging();
builder.Services.AddScoped<IDriverService, DriverService>();


// ✅ Add Swagger with JWT Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyT API", Version = "v1" });
    c.EnableAnnotations();

    // JWT Auth config
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token.\r\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ----------------- Build App ------------------

var app = builder.Build();

// ✅ Identify the current environment
var environment = builder.Environment.IsProduction() ? "Production"
                : builder.Environment.IsStaging() ? "Staging"
                : "Development";

Console.WriteLine($"🌍 Running in **{environment}** mode.");

// ✅ Log Environment Variables & Configurations for Debugging
Console.WriteLine("🔍 Checking loaded configuration values...");

// Log JWT Key
var jwtKey = builder.Configuration["Jwt:Key"];
Console.WriteLine($"🔐 JWT_KEY ({environment}): {(!string.IsNullOrEmpty(jwtKey) ? "✅ Loaded" : "❌ Missing")}");

// Log Database Connection
var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"🔐 DB_CONNECTION ({environment}): {(!string.IsNullOrEmpty(dbConnection) ? "✅ Loaded" : "❌ Missing")}");

Console.WriteLine($"✅ Environment Setup Complete for {environment} mode.");

// ----------------- Configure Error Handling ------------------

var config = app.Services.GetRequiredService<IConfiguration>();
int port = config.GetValue<int>("AppSettings:Port");
Console.WriteLine($"Application running on port: {port}");

// ----------------- Configure Middleware ------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ✅ ensure authentication middleware is added
app.UseAuthorization();

app.MapControllers();

app.Run();
