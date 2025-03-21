namespace Matrix;

public static class Extensions
{
    // Database related services
    public static void AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<MatrixCollegeContext>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IProgressService, ProgressService>();
    }

    // Other services
    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
    }

    // Add CORS policies
    public static void AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });
    }
}
