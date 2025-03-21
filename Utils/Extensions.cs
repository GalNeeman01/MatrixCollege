namespace Matrix;

public static class Extensions
{
    public static void AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<MatrixCollegeContext>();
        services.AddScoped<UserService>();
        services.AddScoped<CourseService>();
        services.AddScoped<LessonService>();
        services.AddScoped<EnrollmentService>();
        services.AddScoped<ProgressService>();
    }

    public static void AddUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<TokenService>();
    }
}
