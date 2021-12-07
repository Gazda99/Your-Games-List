using System.Text;
using Microsoft.IdentityModel.Tokens;
using YGL.API.Settings;

namespace YGL.API.Services; 

public class TokenValidationParametersWithoutLifetime : TokenValidationParameters {
    public TokenValidationParametersWithoutLifetime(JwtSettings jwtSettings) {
        ValidateIssuerSigningKey = true;
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
        ValidateIssuer = false;
        ValidateAudience = false;
        RequireExpirationTime = false;
        ValidateLifetime = false;
    }
}