using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Matrix;

public class TokenService : ITokenService
{
    // DI's
    private readonly AuthSettings _authSettings;

    // Fields
    private readonly JwtSecurityTokenHandler _handler;
    private readonly SymmetricSecurityKey _symmetricSecurityKey; // Must be minimum 16 char string.

    // Constructor
    public TokenService(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));
        _handler = new JwtSecurityTokenHandler();
    }

    // Get a new JWT token for a given username:
    public string GetNewToken(User user)
    {
        var userObject = new Dictionary<string, object>
        {
            { "id", user.Id.ToString() },
            { "name", user.Name },
            { "email", user.Email },
            { "role", user.Role.Name }
        };

        // Claims:
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

        // Descriptor: 
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_authSettings.JWTExpireHours),
            SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512),
            Claims = new Dictionary<string, object>
            {
                { "user", userObject },
                { JwtRegisteredClaimNames.Aud, _authSettings.Audience } // Since this could be a list, add it as a custom claim
            },
            Issuer = _authSettings.Issuer,
        };

        // Return token: 
        SecurityToken securityToken = _handler.CreateToken(descriptor);
        string token = _handler.WriteToken(securityToken);
        return token;
    }
}
