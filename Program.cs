namespace Matrix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AppConfig.Configure();

        builder.Services.AddScoped<MatrixCollegeContext>();
        builder.Services.AddScoped<UserService>();

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
