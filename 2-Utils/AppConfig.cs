namespace Matrix;

public static class AppConfig
{
    public static string ConnectionString { get; set; } = null!;
    public static int JWTExpiresHours { get; set; }

                                            // Random 71 character long string as a key
    public static string JWTKey = "Ott0DMyROeZUJiO3X7eIdY52a91nePq6GjGxEv63wzRBoiRg7GtuYqH9/PGMvndt\r\nxktvjzxpi+9RFOP9JQBZQw==";

    public static void Configure()
    {
        IConfigurationRoot settings = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json")
            .AddJsonFile($"appsettings.Development.json")
            .Build();

        ConnectionString = settings.GetConnectionString("MatrixCollege")!;

        JWTKey = settings.GetSection("JwtSettings").GetValue<string>("Secret")!;
        JWTExpiresHours = settings.GetSection("JwtSettings").GetValue<int>("JwtExpireHours")!;
    }
}
