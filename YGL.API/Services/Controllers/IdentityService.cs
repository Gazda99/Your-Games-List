using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YGL.API.Controllers.V1;
using YGL.API.Domain;
using YGL.API.EnumTypes;
using YGL.API.Errors;
using YGL.API.Services.IControllers;
using YGL.API.Settings;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers;

public partial class IdentityService : IIdentityService {
    private const int RefreshTokenSize = 64;
    private const int ResetPasswordTokenSize = 64;

    private readonly YGL.Model.YGLDataContext _yglDataContext;

    private readonly ICustomPasswordHasher _customPasswordHasher;

    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParametersWithoutLifetime _tokenValidationParametersWithoutLifetime;

    private readonly ConfirmationEmailSettings _confirmationEmailSettings;
    private readonly PasswordResetEmailSettings _passwordResetEmailSettings;

    private readonly EmailSender<AccountConfirmationEmailSender> _accountConfirmationEmailSender;
    private readonly EmailSender<PasswordResetEmailSender> _passwordResetEmailSender;

    private readonly PasswordResetSettings _passwordResetSettings;

    public IdentityService(YGL.Model.YGLDataContext yglDataContext, ICustomPasswordHasher customPasswordHasher,
        JwtSettings jwtSettings, TokenValidationParametersWithoutLifetime tokenValidationParametersWithoutLifetime,
        ConfirmationEmailSettings confirmationEmailSettings,
        PasswordResetEmailSettings passwordResetEmailSettings,
        EmailSender<AccountConfirmationEmailSender> accountConfirmationEmailSender,
        EmailSender<PasswordResetEmailSender> passwordResetEmailSender,
        PasswordResetSettings passwordResetSettings) {
        _yglDataContext = yglDataContext;
        _customPasswordHasher = customPasswordHasher;
        _jwtSettings = jwtSettings;
        _tokenValidationParametersWithoutLifetime = tokenValidationParametersWithoutLifetime;
        _confirmationEmailSettings = confirmationEmailSettings;
        _passwordResetEmailSettings = passwordResetEmailSettings;
        _accountConfirmationEmailSender = accountConfirmationEmailSender;
        _passwordResetEmailSender = passwordResetEmailSender;
        _passwordResetSettings = passwordResetSettings;
    }

    public async Task<AuthenticationResult> RegisterAsync(
        string email, string username, string password) {
        var authResult = new AuthenticationResult();

        if (!ValidationUser.ValidateUsername(username, authResult)) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return authResult;
        }

        if (!ValidationUser.ValidateEmail(email, authResult)) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return authResult;
        }

        if (!ValidationUser.ValidatePassword(password, authResult)) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return authResult;
        }

        if (password == username) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameAndPasswordTheSame);
            return authResult;
        }


        //check if username is taken
        var foundUser = await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.ItemStatus == true);

        if (foundUser is not null) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameAlreadyInUse);
            authResult.StatusCode = HttpStatusCode.Conflict;
            return authResult;
        }

        //check if email is taken
        foundUser = await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.ItemStatus == true);

        if (foundUser is not null) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.EmailAlreadyInUse);
            authResult.StatusCode = HttpStatusCode.Conflict;
            return authResult;
        }

        //everything is good, create user

        byte[] hashedPassword = _customPasswordHasher.HashPassword(password, out byte[] salt);

        YGL.Model.User newUser = new YGL.Model.User() {
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            BirthYear = -1, //not specified
            About = string.Empty,
            IsEmailConfirmed = false,
            Gender = (byte)Gender.Default, //not specified
            Country = (short)Country.Default, //not specified
            Rank = 0,
            Experience = 0,
            LastLogin = null,
        };

        //create default role for user
        YGL.Model.UserWithRole userWithRole = new YGL.Model.UserWithRole() {
            Role = (int)Role.Default,
            User = newUser
        };


        await _yglDataContext.Users.AddAsync(newUser);
        await _yglDataContext.UserWithRoles.AddAsync(userWithRole);
        await _yglDataContext.SaveChangesAsync();

        var isConfirmationEmailSent = await SendConfirmationEmailAsync(newUser.Id, newUser.Username, newUser.Email);

        if (!isConfirmationEmailSent) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.InternalServerError;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CouldNotSentEmailConfirmation);
            return authResult;
        }

        authResult.Location = Routes.User.GetLocation(newUser.Id);
        authResult.IsSuccess = true;
        authResult.StatusCode = HttpStatusCode.Created;
        return authResult;
    }


    public async Task<AuthenticationResult> LoginAsync(string username, string password) {
        var authResult = new AuthenticationResult();

        if (!ValidationUser.ValidateUsername(username, authResult)) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return authResult;
        }

        if (!ValidationUser.ValidatePassword(password, authResult)) {
            authResult.IsSuccess = false;
            authResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return authResult;
        }

        var foundUser = await _yglDataContext.Users
            .Include(e => e.UserWithRoles)
            .FirstOrDefaultAsync(u => u.Username == username && u.ItemStatus == true);

        if (foundUser is null) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameNotFound);
            authResult.StatusCode = HttpStatusCode.Unauthorized;
            return authResult;
        }

        //email confirmation
        if (foundUser.IsEmailConfirmed == false) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.EmailNotConfirmed);
            authResult.StatusCode = HttpStatusCode.Unauthorized;
            return authResult;
        }


        var isPasswordCorrect =
            _customPasswordHasher.VerifyPassword(password, foundUser.HashedPassword, foundUser.Salt);

        if (!isPasswordCorrect) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.IncorrectPassword);
            authResult.StatusCode = HttpStatusCode.Unauthorized;
            return authResult;
        }

        var tokensToRevoke =
            await _yglDataContext.RefreshTokens.Where(t =>
                t.UserId == foundUser.Id && t.IsRevoked == false && t.IsUsed == false).ToListAsync();

        foreach (var tokenToRevoke in tokensToRevoke) {
            tokenToRevoke.IsRevoked = true;
            _yglDataContext.RefreshTokens.Update(tokenToRevoke);
        }


        foundUser.LastLogin = DateTime.UtcNow;
        _yglDataContext.Users.Update(foundUser);

        await _yglDataContext.SaveChangesAsync();

        authResult = await GenerateTokens(foundUser);
        authResult.IsSuccess = true;
        authResult.StatusCode = HttpStatusCode.OK;

        return authResult;
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken) {
        AuthenticationResult authResult = await VerifyAndGenerateTokens(token, refreshToken);
        return authResult;
    }

    private async Task<AuthenticationResult> GenerateTokens(YGL.Model.User user) {
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(CreateClaims(user)),
            Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.TokenLifeTime),
            SigningCredentials = new SigningCredentials(mySecurityKey, _jwtSettings.SecurityAlgorithm)
        };

        SecurityToken jwtToken = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = new YGL.Model.RefreshToken() {
            JwtId = jwtToken.Id,
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshTokenLifeTime),
            Token = GenerateRefreshToken()
        };


        await _yglDataContext.RefreshTokens.AddAsync(refreshToken);
        await _yglDataContext.SaveChangesAsync();

        return new AuthenticationResult() {
            UserId = user.Id,
            JwtToken = tokenHandler.WriteToken(jwtToken),
            RefreshToken = refreshToken.Token,
            IsSuccess = true
        };
    }


    private async Task<AuthenticationResult> VerifyAndGenerateTokens(string jwtToken, string refreshToken) {
        var authResult = new AuthenticationResult();

        var tokenHandler = new JwtSecurityTokenHandler();

        try {
            //validate token (without checking expiration time)
            ClaimsPrincipal jwtTokenInVerification = tokenHandler.ValidateToken(jwtToken,
                _tokenValidationParametersWithoutLifetime, out SecurityToken validatedToken);

            if (!IsJwtWithValidSecurityAlgo(validatedToken))
                return ReturnTokenValidationError(ApiErrorCodes.JwtTokenValidationError);

            if (IsJwtNotYetExpired(jwtTokenInVerification))
                return ReturnTokenValidationError(ApiErrorCodes.JwtTokenNotYetExpired);

            //get refresh token from DB
            var storedRefreshToken =
                await _yglDataContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (storedRefreshToken is null)
                return ReturnTokenValidationError(ApiErrorCodes.RefreshTokenValidationError);

            if (storedRefreshToken.IsUsed)
                return ReturnTokenValidationError(ApiErrorCodes.RefreshTokenValidationError);

            if (storedRefreshToken.IsRevoked)
                return ReturnTokenValidationError(ApiErrorCodes.RefreshTokenValidationError);

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return ReturnTokenValidationError(ApiErrorCodes.RefreshTokenExpired);

            string jti = jwtTokenInVerification.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (storedRefreshToken.JwtId != jti)
                return ReturnTokenValidationError(ApiErrorCodes.RefreshTokenValidationError);

            //now everything is checked and is good
            //update current token
            storedRefreshToken.IsUsed = true;
            _yglDataContext.RefreshTokens.Update(storedRefreshToken);
            await _yglDataContext.SaveChangesAsync();

            //find user based on provided token
            var foundUser = await _yglDataContext.Users
                .Include(e => e.UserWithRoles)
                .FirstOrDefaultAsync(u => u.Id == storedRefreshToken.UserId && u.ItemStatus == true);

            //generate new token for that user
            AuthenticationResult generateTokenResult = await GenerateTokens(foundUser);
            generateTokenResult.StatusCode =
                generateTokenResult.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;

            return generateTokenResult;
        }
        catch (Exception) {
            return ReturnTokenValidationError(ApiErrorCodes.JwtTokenValidationError);
        }

        AuthenticationResult ReturnTokenValidationError(ApiErrorCodes errorCode) {
            authResult.IsSuccess = false;
            authResult.AddErrors<ApiErrors, ApiErrorCodes>(errorCode);
            authResult.StatusCode = HttpStatusCode.Unauthorized;
            return authResult;
        }
    }

    public async Task<EmailConfirmationResult> ConfirmEmailAsync(string url) {
        var emailConfirmationResult = new EmailConfirmationResult();

        if (string.IsNullOrEmpty(url)) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailConfirmationNotValid);
            return emailConfirmationResult;
        }


        string[] urlSplit;
        long userId;
        try {
            urlSplit = url.Split('_', 2);
            userId = long.Parse(urlSplit[0]);
        }
        catch (Exception) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.Unauthorized;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailConfirmationNotValid);
            return emailConfirmationResult;
        }

        var foundUser = _yglDataContext.Users.FirstOrDefault(u => u.Id == userId && u.ItemStatus == true);

        if (foundUser is null) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.Unauthorized;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailConfirmationNotValid);
            return emailConfirmationResult;
        }

        if (foundUser.IsEmailConfirmed) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.Conflict;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailAlreadyConfirmed);
            return emailConfirmationResult;
        }

        var emailConfirmations = await _yglDataContext.EmailConfirmations
            .Where(e => e.UserId == userId && e.IsUsed == false)
            .ToListAsync();

        foreach (var emailConfirmation in emailConfirmations) {
            //check if url is the same
            if (emailConfirmation.Url != urlSplit[1]) continue;

            //check if is expired
            if (emailConfirmation.ExpiryDate < DateTime.UtcNow) {
                emailConfirmationResult.IsSuccess = false;
                emailConfirmationResult.StatusCode = HttpStatusCode.Unauthorized;
                emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                    .EmailConfirmationExpired);

                emailConfirmation.IsUsed = true;
                _yglDataContext.EmailConfirmations.Update(emailConfirmation);
                await _yglDataContext.SaveChangesAsync();

                return emailConfirmationResult;
            }


            foundUser.IsEmailConfirmed = true;
            emailConfirmation.IsUsed = true;

            _yglDataContext.Users.Update(foundUser);
            _yglDataContext.EmailConfirmations.Update(emailConfirmation);

            await _yglDataContext.SaveChangesAsync();

            emailConfirmationResult.StatusCode = HttpStatusCode.OK;
            emailConfirmationResult.IsSuccess = true;
            return emailConfirmationResult;
        }

        emailConfirmationResult.IsSuccess = false;
        emailConfirmationResult.StatusCode = HttpStatusCode.Unauthorized;
        emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
            .EmailConfirmationNotValid);
        return emailConfirmationResult;
    }


    public async Task<EmailConfirmationResult> ResendConfirmationEmailAsync(string email) {
        var emailConfirmationResult = new EmailConfirmationResult();

        if (!ValidationUser.ValidateEmail(email, emailConfirmationResult)) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return emailConfirmationResult;
        }

        var foundUser = _yglDataContext.Users.FirstOrDefault(u => u.Email == email && u.ItemStatus == true);

        if (foundUser is null) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.Unauthorized;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailNotFound);
            return emailConfirmationResult;
        }

        if (foundUser.IsEmailConfirmed) {
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.Conflict;
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .EmailAlreadyConfirmed);
            return emailConfirmationResult;
        }

        var emailConfirmations = await _yglDataContext.EmailConfirmations
            .Where(e => e.UserId == foundUser.Id && e.IsUsed == false)
            .ToListAsync();

        foreach (var emailConfirmation in emailConfirmations) {
            emailConfirmation.IsUsed = true;
        }

        _yglDataContext.EmailConfirmations.UpdateRange(emailConfirmations);
        await _yglDataContext.SaveChangesAsync();


        var isConfirmationEmailSent = await SendConfirmationEmailAsync(foundUser.Id, foundUser.Username, foundUser.Email);

        if (!isConfirmationEmailSent) {
            emailConfirmationResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .CouldNotSentEmailConfirmation);
            emailConfirmationResult.IsSuccess = false;
            emailConfirmationResult.StatusCode = HttpStatusCode.InternalServerError;
            return emailConfirmationResult;
        }

        emailConfirmationResult.IsSuccess = true;
        emailConfirmationResult.StatusCode = HttpStatusCode.OK;
        return emailConfirmationResult;
    }

    public async Task<PasswordResetResult> SendResetPasswordEmailAsync(string email) {
        var passwordResetResult = new PasswordResetResult();

        if (!ValidationUser.ValidateEmail(email, passwordResetResult)) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return passwordResetResult;
        }

        var foundUser = await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.ItemStatus == true);

        if (foundUser is null) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
            passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.EmailNotFound);
            return passwordResetResult;
        }

        var passwordResets = await _yglDataContext.PasswordResets
            .Where(e => e.UserId == foundUser.Id && e.IsUsed == false)
            .ToListAsync();

        foreach (var passwordReset in passwordResets) {
            passwordReset.IsUsed = true;
        }

        _yglDataContext.PasswordResets.UpdateRange(passwordResets);
        await _yglDataContext.SaveChangesAsync();


        var isResetPasswordEmailSent = await SendResetPasswordEmailAsync(foundUser.Id, foundUser.Username, foundUser.Email);

        if (!isResetPasswordEmailSent) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.InternalServerError;
            passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CouldNotSentResetPasswordEmail);
            return passwordResetResult;
        }

        passwordResetResult.IsSuccess = true;
        passwordResetResult.StatusCode = HttpStatusCode.OK;
        return passwordResetResult;
    }

    public async Task<PasswordResetResult> ConfirmResetPasswordAsync(string url) {
        var passwordResetResult = new PasswordResetResult();

        if (string.IsNullOrEmpty(url)) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PasswordResetEmailNotValid);
            return passwordResetResult;
        }

        string[] urlSplit;
        long userId;
        try {
            urlSplit = url.Split('_', 2);
            userId = long.Parse(urlSplit[0]);
        }
        catch (Exception) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
            passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .PasswordResetEmailNotValid);
            return passwordResetResult;
        }

        var foundUser = _yglDataContext.Users.FirstOrDefault(u => u.Id == userId && u.ItemStatus == true);

        if (foundUser is null) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
            passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .PasswordResetEmailNotValid);
            return passwordResetResult;
        }

        var passwordResets = await _yglDataContext.PasswordResets
            .Where(p => p.UserId == userId && p.IsUsed == false)
            .ToListAsync();


        foreach (var passwordReset in passwordResets) {
            if (passwordReset.Url != urlSplit[1]) continue;

            //check if is expired
            if (passwordReset.ExpiryDate < DateTime.UtcNow) {
                passwordResetResult.IsSuccess = false;
                passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
                passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                    .EmailConfirmationExpired);

                passwordReset.IsUsed = true;
                _yglDataContext.PasswordResets.Update(passwordReset);
                await _yglDataContext.SaveChangesAsync();

                return passwordResetResult;
            }

            passwordReset.IsUsed = true;
            _yglDataContext.PasswordResets.Update(passwordReset);

            string passwordResetToken = GenerateResetPasswordToken();


            //clear all other reset tokens
            var passwordResets2 = await _yglDataContext.PasswordReset2s
                .Where(p => p.UserId == userId && p.IsUsed == false && p.ItemStatus == true)
                .ToListAsync();

            foreach (var passwordResets2InDb in passwordResets2)
                passwordResets2InDb.IsUsed = true;

            _yglDataContext.PasswordReset2s.UpdateRange(passwordResets2);


            var passwordReset2 = new YGL.Model.PasswordReset2() {
                Token = passwordResetToken,
                UserId = foundUser.Id,
                ExpiryDate = DateTime.UtcNow.AddSeconds(_passwordResetSettings.TokenLifeTime),
                IsUsed = false
            };

            await _yglDataContext.PasswordReset2s.AddAsync(passwordReset2);
            await _yglDataContext.SaveChangesAsync();

            passwordResetResult.IsSuccess = true;
            passwordResetResult.StatusCode = HttpStatusCode.OK;
            passwordResetResult.PasswordResetToken = passwordResetToken;
            return passwordResetResult;
        }

        passwordResetResult.IsSuccess = false;
        passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
        passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
            .PasswordResetEmailNotValid);
        return passwordResetResult;
    }

    public async Task<PasswordResetResult> ResetPasswordAsync(string passwordResetToken, string newPassword) {
        var passwordResetResult = new PasswordResetResult();

        if (!ValidationUser.ValidatePassword(newPassword, passwordResetResult)) {
            passwordResetResult.IsSuccess = false;
            passwordResetResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return passwordResetResult;
        }

        var passwordReset2Found = await _yglDataContext.PasswordReset2s.Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Token == passwordResetToken && p.IsUsed == false && p.ItemStatus == true);

        if (passwordReset2Found is not null) {
            if (passwordReset2Found.ExpiryDate < DateTime.UtcNow) {
                passwordResetResult.IsSuccess = false;
                passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
                passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                    .PasswordResetTokenExpired);

                passwordReset2Found.IsUsed = true;
                _yglDataContext.PasswordReset2s.Update(passwordReset2Found);
                await _yglDataContext.SaveChangesAsync();

                return passwordResetResult;
            }

            byte[] hashedPassword = _customPasswordHasher.HashPassword(newPassword, out byte[] salt);

            if (Helpers.ArrayHelper.CompareTwoByteArrays(hashedPassword, passwordReset2Found.User.HashedPassword)) {
                passwordResetResult.IsSuccess = false;
                passwordResetResult.StatusCode = HttpStatusCode.UnprocessableEntity;
                passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                    .PasswordIsSameAsBefore);
                return passwordResetResult;
            }


            if (newPassword == passwordReset2Found.User.Username) {
                passwordResetResult.IsSuccess = false;
                passwordResetResult.StatusCode = HttpStatusCode.UnprocessableEntity;
                passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                    .UsernameAndPasswordTheSame);
                return passwordResetResult;
            }

            passwordReset2Found.IsUsed = true;

            var user = passwordReset2Found.User;

            user.HashedPassword = hashedPassword;
            user.Salt = salt;

            _yglDataContext.Users.Update(user);
            _yglDataContext.PasswordReset2s.Update(passwordReset2Found);
            await _yglDataContext.SaveChangesAsync();

            passwordResetResult.IsSuccess = true;
            passwordResetResult.StatusCode = HttpStatusCode.OK;
            return passwordResetResult;
        }

        passwordResetResult.IsSuccess = false;
        passwordResetResult.StatusCode = HttpStatusCode.Unauthorized;
        passwordResetResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PasswordResetTokenNotValid);
        return passwordResetResult;
    }
}