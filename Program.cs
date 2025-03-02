using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AppConfig.Configure();

        builder.Services.AddScoped<MatrixCollegeContext>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<EnrollmentService>();
        builder.Services.AddScoped<LessonService>();
        builder.Services.AddScoped<ProgressService>();

        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

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
