namespace Matrix;

public static class Extensions
{
    // Database related services
    public static void AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<MatrixCollegeContext>();
        services.AddScoped<UserService>();
        services.AddScoped<CourseService>();
        services.AddScoped<LessonService>();
        services.AddScoped<EnrollmentService>();
        services.AddScoped<ProgressService>();
    }

    // Other services
    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<TokenService>();
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
