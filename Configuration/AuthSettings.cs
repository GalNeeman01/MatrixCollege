namespace Matrix;

public class AuthSettings
{
    public string Secret { get; set; } = string.Empty;

    public int JWTExpireHours { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public List<string> Audience { get; set; } = new List<string>();
}
