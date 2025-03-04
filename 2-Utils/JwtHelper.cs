using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Matrix;

public static class JwtHelper
{
    private static readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
    private static readonly SymmetricSecurityKey _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.JWTKey)); // Must be minimum 16 char string.

    // Get a new JWT token for a given username:
    public static string GetNewToken(User user)
    {
        // Create JSON:
        var slimUser = new { user.Id, user.Email, user.RoleId, Role = user.Role.Name };
        string json = JsonSerializer.Serialize(slimUser, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        // Claims:
        List<Claim> claims = new List<Claim> { 
            new Claim(ClaimTypes.Actor, json),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        // Descriptor: 
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(AppConfig.JWTExpiresHours),
            SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512)
        };

        // Return token: 
        SecurityToken securityToken = _handler.CreateToken(descriptor);
        string token = _handler.WriteToken(securityToken);
        return token;
    }

    // Set default bearer options: 
    public static void SetBearerOptions(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // We didn't set an Issuer claim (which server/microservice issue the token), so don't validate it (otherwise validation failed).
            ValidateAudience = false, // We didn't set an Audience claim (which server our audience browse to), so don't validate it (otherwise validation failed).
            ValidateIssuerSigningKey = true, // Validate the secret key.
            IssuerSigningKey = _symmetricSecurityKey // The secret key to validate.
        };
    }
}
