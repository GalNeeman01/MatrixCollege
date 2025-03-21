using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Matrix;

public class JwtBearerOptionsSetup
{
    // Set default bearer options: 
    public static void Configure(JwtBearerOptions options, AuthSettings authSettings)
    {
        SecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // We didn't set an Issuer claim (which server/microservice issue the token), so don't validate it (otherwise validation failed).
            ValidateAudience = false, // We didn't set an Audience claim (which server our audience browse to), so don't validate it (otherwise validation failed).
            ValidateIssuerSigningKey = true, // Validate the secret key.
            IssuerSigningKey = symmetricSecurityKey // The secret key to validate.
        };
    }
}
