using AspNetCoreRateLimit;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Runtime;

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

        // Setup appconfig
        AppConfig.Configure();

        // Setup ratelimit
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("RateLimit"));
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddInMemoryRateLimiting();

        // Add DI services
        builder.Services.AddScoped<MatrixCollegeContext>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<LessonService>();
        builder.Services.AddScoped<EnrollmentService>();
        builder.Services.AddScoped<ProgressService>();

        // Add Fluent DI validators
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CourseValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LessonValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<EnrollmentValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProgressValidator>();

        // IOptions DIs
        builder.Services.Configure<LogSettings>(
            builder.Configuration.GetSection("LogSettings"));

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
        builder.Services.AddMvc(options => options.Filters.Add<CatchAllMiddleware>());

        // Ignore EF ModelState input validation (To allow Fluent to work)
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtHelper.SetBearerOptions);

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
