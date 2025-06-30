using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// ----------------- Configure Services ------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------- Build App ------------------

var app = builder.Build();

// ----------------- Configure Middleware ------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
