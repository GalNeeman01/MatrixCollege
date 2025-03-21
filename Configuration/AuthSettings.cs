namespace Matrix;

public class AuthSettings
{
    public string Secret { get; set; } = string.Empty;

    public int JWTExpireHours { get; set; }
}
