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

        // Add DI objects
        builder.Services.AddScoped<MatrixCollegeContext>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<EnrollmentService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<ProgressService>();
        builder.Services.AddScoped<LessonService>();

        // Add Fluent DI objects
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CourseValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LessonValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<EnrollmentValidator>();

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
