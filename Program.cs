using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Setup appconfig
        AppConfig.Configure();

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

        // Ignore EF ModelState input validation (To allow Fluent to work)
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
