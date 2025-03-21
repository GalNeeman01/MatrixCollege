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
            ValidateIssuer = true, // Validate issuer
            ValidateAudience = true, // Validate audience
            ValidateIssuerSigningKey = true, // Validate the secret key.
            IssuerSigningKey = symmetricSecurityKey, // The secret key to validate.
            ValidIssuer = authSettings.Issuer, // Retrieve valid issuer from configuration file
            ValidAudiences = authSettings.Audience // Retrieve valid audiences from configuration file
        };
    }
}
