using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using YGL.API.EnumTypes;
using YGL.API.Exceptions;
using YGL.API.Helpers;

namespace YGL.API.Services.Controllers;

public partial class IdentityService {
    private static string GenerateRefreshToken() {
        var randomBytes = RngHelper.GenerateRandomByteArray(RefreshTokenSize);
        var refreshToken = Convert.ToBase64String(randomBytes);
        return refreshToken;
    }

    private static IEnumerable<Claim> CreateClaims(YGL.Model.User user) {
        IEnumerable<Claim> roleClaims =
            user.UserWithRoles.Select(r => new Claim(ClaimTypes.Role, Role.GetRoleValueOrDefault(r.Role)));

        var claims = new List<Claim>();
        claims.Add(new Claim("Id", user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.AddRange(roleClaims);

        return claims;
    }

    private static bool IsJwtWithValidSecurityAlgo(SecurityToken validatedToken) {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool IsJwtNotYetExpired(ClaimsPrincipal tokenInVerification) {
        var jwtExpiryDateString = tokenInVerification.Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

        if (string.IsNullOrEmpty(jwtExpiryDateString))
            throw new ArgumentNullException();

        long utcExpiryDate = long.Parse(jwtExpiryDateString);

        var expiryDate = DateTime.UnixEpoch.AddSeconds(utcExpiryDate);

        return expiryDate > DateTime.UtcNow;
    }


    private async Task<bool> SendConfirmationEmailAsync(long userId, string username, string email) {
        string confirmationUrl;
        try {
            confirmationUrl = await _accountConfirmationEmailSender.SendEmailAndGetUrlAsync(username, email, userId);
        }
        catch (CannotSendEmailException) {
            return false;
        }

        var emailConfirmation = new YGL.Model.EmailConfirmation() {
            Url = confirmationUrl,
            ExpiryDate = DateTime.UtcNow.AddSeconds(_confirmationEmailSettings.UrlLifeTime),
            UserId = userId,
            IsUsed = false
        };

        await _yglDataContext.EmailConfirmations.AddAsync(emailConfirmation);
        await _yglDataContext.SaveChangesAsync();

        return true;
    }

    private async Task<bool> SendResetPasswordEmailAsync(long userId, string username, string email) {
        string confirmationUrl;
        try {
            confirmationUrl =
                await _passwordResetEmailSender.SendEmailAndGetUrlAsync(username, email, userId);
        }
        catch (CannotSendEmailException) {
            return false;
        }

        var passwordReset = new YGL.Model.PasswordReset() {
            Url = confirmationUrl,
            ExpiryDate = DateTime.UtcNow.AddSeconds(_passwordResetEmailSettings.UrlLifeTime),
            UserId = userId,
            IsUsed = false
        };

        await _yglDataContext.PasswordResets.AddAsync(passwordReset);
        await _yglDataContext.SaveChangesAsync();

        return true;
    }

    private static string GenerateResetPasswordToken() {
        var randomBytes = RngHelper.GenerateRandomByteArray(ResetPasswordTokenSize);
        var refreshToken = Convert.ToBase64String(randomBytes);
        return refreshToken;
    }
}