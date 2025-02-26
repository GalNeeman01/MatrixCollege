namespace Matrix;

public static class AppConfig
{
    public static string ConnectionString { get; set; } = null!;

    public static void Configure()
    {
        IConfigurationRoot settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        ConnectionString = settings.GetConnectionString("MatrixCollege")!;
    }
}
