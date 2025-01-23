using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeDepositAPI.Services;
using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(options =>
{
    // Automatically register validators from this assembly.
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});
// Register the DbContext with PostgreSQL provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register JWT Token Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITopUpService, TopUpService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRolloverService, RolloverService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    options.Events = new JwtBearerEvents
    {
    //      OnMessageReceived = context =>
    //      {
    //          return null;
    //      },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated for " + context.Principal.Identity.Name);
            return Task.CompletedTask;
        }
        // Other event handlers as needed...
    };
});

// Configure Hangfire to use PostgreSQL storage
builder.Services.AddHangfire(configuration => configuration
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Schedule recurring job
RecurringJob.AddOrUpdate<IRolloverService>(
    "process-matured-deposits",
    service => service.ProcessMaturedDepositsAsync(),
    Cron.Minutely);

app.MapControllers();  // Add this line to map controller endpoints

app.Run();
