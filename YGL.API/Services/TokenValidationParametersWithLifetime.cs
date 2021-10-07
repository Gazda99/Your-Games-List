using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using YGL.API.Settings;

namespace YGL.API.Services {
public class TokenValidationParametersWithLifetime : TokenValidationParameters {
    public TokenValidationParametersWithLifetime(JwtSettings jwtSettings) {
        ValidateIssuerSigningKey = true;
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
        ValidateIssuer = false;
        ValidateAudience = false;
        RequireExpirationTime = false;
        ValidateLifetime = true;
        ClockSkew = TimeSpan.Zero;
    }
}
}