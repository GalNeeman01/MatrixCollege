using AspNetCoreRateLimit;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Matrix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Setup Serilog Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Host.UseSerilog();

        // Use AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Setup ratelimit
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("RateLimit"));
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddInMemoryRateLimiting();

        // Add DI services
        builder.Services.AddDbServices();
        builder.Services.AddUtilityServices();

        // Add Fluent DI validators
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        
        // IOptions DIs
        builder.Services.Configure<LogSettings>(
            builder.Configuration.GetSection(nameof(LogSettings)));

        builder.Services.Configure<AuthSettings>(
            builder.Configuration.GetSection(nameof(AuthSettings)));

        builder.Services.Configure<DatabaseSettings>(
            builder.Configuration.GetSection(nameof(DatabaseSettings)));

        // Add jobs
        builder.Services.AddHostedService<LogCleanerService>();

        // Add CORS Policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        // Add global filters
        builder.Services.AddMvc(options => options.Filters.Add<CatchAllFilter>());

        // Ignore EF ModelState input validation (To allow Fluent to work)
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            AuthSettings authSettings = builder.Configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>()!;
            JwtBearerOptionsSetup.Configure(options, authSettings);
        });

        builder.Services.AddControllers();

        var app = builder.Build();

        // Apply CORS
        app.UseCors("AllowAll");

        // Middleware
        app.UseIpRateLimiting();
        app.UseSerilogRequestLogging();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
