using System.Collections.Generic;

namespace YGL.API.Errors; 

public class ApiErrors : IErrorContainer<ApiErrorCodes> {
    public Dictionary<ApiErrorCodes, string> Errors { get; set; } = new Dictionary<ApiErrorCodes, string>() {
        { ApiErrorCodes.JwtTokenValidationError, "Provided JWT token could not be validated." },
        { ApiErrorCodes.JwtTokenExpired, "Provided JWT token has expired." },
        { ApiErrorCodes.JwtTokenIsUsed, "Provided JWT token is used." },
        { ApiErrorCodes.JwtTokenIsRevoked, "Provided JWT token is revoked." },
        { ApiErrorCodes.JwtTokenNotYetExpired, "Provided JWT token has not expired yet." },

        { ApiErrorCodes.RefreshTokenValidationError, "Provided refresh token could not be validated." },
        { ApiErrorCodes.RefreshTokenExpired, "Provided refresh token has expired." },

        { ApiErrorCodes.UserNotFound, "User not found." },
        { ApiErrorCodes.UsernameAlreadyInUse, "Provided username is already in use." },

        { ApiErrorCodes.UsernameNotFound, "Username with provided username not found." },
        { ApiErrorCodes.UsernameDoesNotMatchCriteria, "Provided username does not match criteria." },
        { ApiErrorCodes.UsernameIsTooShort, "Provided username is too short." },
        { ApiErrorCodes.UsernameIsTooLong, "Provided username is too long." },
        { ApiErrorCodes.UsernameAndPasswordTheSame, "Provided username and password cannot be the same." },

        { ApiErrorCodes.PasswordDoesNotMatchCriteria, "Provided password does not match criteria." },
        { ApiErrorCodes.PasswordIsTooShort, "Provided password is too short." },
        { ApiErrorCodes.PasswordIsTooLong, "Provided password is too long." },
        { ApiErrorCodes.PasswordIsSameAsBefore, "Provided password is same as before." },
        { ApiErrorCodes.IncorrectPassword, "Provided password is not correct." },

        { ApiErrorCodes.EmailDoesNotMatchCriteria, "Provided email is not a valid one." },
        { ApiErrorCodes.EmailIsTooShort, "Provided email is too short." },
        { ApiErrorCodes.EmailIsTooLong, "Provided email is too long." },
        { ApiErrorCodes.EmailDomainBlacklisted, "Provided email has a domain which is blacklisted." },
        { ApiErrorCodes.EmailNotFound, "Provided email not found." },
        { ApiErrorCodes.EmailAlreadyInUse, "Email already is in use." },
        { ApiErrorCodes.EmailNotConfirmed, "Email not confirmed" },

        { ApiErrorCodes.AboutIsTooShort, "About section is too short." },
        { ApiErrorCodes.AboutIsTooLong, "About section is too long." },

        { ApiErrorCodes.InvalidCountry, "Invalid country." },

        { ApiErrorCodes.InvalidGender, "Invalid gender." },

        { ApiErrorCodes.BirthYearTooLow, "Birth year is too low." },
        { ApiErrorCodes.BirthYearTooHigh, "Birth year is too high." },

        { ApiErrorCodes.SlugIsTooShort, "Slug is too short." },
        { ApiErrorCodes.SlugIsTooLong, "Slug is too long." },
        { ApiErrorCodes.SlugIsAlreadyTaken, "Slug is already taken." },

        { ApiErrorCodes.EmailConfirmationIsUsed, "Email confirmation is used." },
        { ApiErrorCodes.EmailAlreadyConfirmed, "Email is already confirmed." },

        { ApiErrorCodes.EmailConfirmationExpired, "Email confirmation expired." },
        { ApiErrorCodes.EmailConfirmationNotValid, "Email confirmation is not valid." },
        { ApiErrorCodes.CouldNotSentEmailConfirmation, "Could not sent email - account confirmation." },

        { ApiErrorCodes.PasswordResetEmailNotValid, "Reset password email is not valid" },
        { ApiErrorCodes.PasswordResetEmailExpired, "Reset password email expired." },
        { ApiErrorCodes.PasswordResetEmailIsUsed, "Reset password email is used." },
        { ApiErrorCodes.CouldNotSentResetPasswordEmail, "Could not sent email - reset password." },

        { ApiErrorCodes.PasswordResetTokenNotValid, "Reset password token is not valid." },
        { ApiErrorCodes.PasswordResetTokenExpired, "Reset password token expired." },
        { ApiErrorCodes.PasswordResetTokenIsUsed, "Reset password token is used." },


        { ApiErrorCodes.CannotParseIdsInUrlBadValue, "Cannot parse provided id list - bad value provided." },
        { ApiErrorCodes.CannotParseIdsInUrlTooManyIds, "Cannot parse provided id list - too many ids provided." },
        { ApiErrorCodes.CannotParseIdsInUrlNonPositiveId, "Cannot parse provided id list - id must be > 0." },

        { ApiErrorCodes.NotFound, "Resource not found." },
        { ApiErrorCodes.Forbidden, "Resource is forbidden." },
        { ApiErrorCodes.CompanyNotFound, "Company not found" },
        { ApiErrorCodes.GenreNotFound, "Genre not found." },
        { ApiErrorCodes.GameModeNotFound, "Game mode not found." },
        { ApiErrorCodes.PlatformNotFound, "Platform not found." },
        { ApiErrorCodes.PlayerPerspectiveNotFound, "Player perspective not found." },
    };
}

public enum ApiErrorCodes {
    JwtTokenValidationError = 1100_50_01,
    JwtTokenExpired = 1100_50_02,
    JwtTokenIsUsed = 1100_41_01,
    JwtTokenIsRevoked = 1100_50_03,
    JwtTokenNotYetExpired = 1100_50_04,

    RefreshTokenValidationError = 1101_50_01,
    RefreshTokenExpired = 1101_50_02,

    UserNotFound = 1200_40_01,
    UsernameAlreadyInUse = 1201_41_01,

    UsernameNotFound = 1201_40_01,
    UsernameDoesNotMatchCriteria = 1201_30_02,
    UsernameIsTooShort = 1201_30_03,
    UsernameIsTooLong = 1201_30_04,
    UsernameAndPasswordTheSame = 1201_30_05,

    PasswordDoesNotMatchCriteria = 1202_30_02,
    PasswordIsTooShort = 1202_30_03,
    PasswordIsTooLong = 1202_30_04,
    PasswordIsSameAsBefore = 1202_30_10,
    IncorrectPassword = 1202_50_01,

    EmailDoesNotMatchCriteria = 1203_30_02,
    EmailIsTooShort = 1203_30_03,
    EmailIsTooLong = 1203_30_04,
    EmailDomainBlacklisted = 1203_30_05,
    EmailNotFound = 1203_40_01,
    EmailAlreadyInUse = 1203_41_01,
    EmailNotConfirmed = 1203_50_01,

    AboutIsTooShort = 1210_30_03,
    AboutIsTooLong = 1210_30_04,

    InvalidCountry = 1211_30_02,

    InvalidGender = 1212_30_02,

    BirthYearTooLow = 1213_30_03,
    BirthYearTooHigh = 1213_30_04,

    SlugIsTooShort = 1214_30_03,
    SlugIsTooLong = 1214_30_04,
    SlugIsAlreadyTaken = 1214_41_01,

    EmailConfirmationIsUsed = 1300_41_01,
    EmailAlreadyConfirmed = 1300_41_11,

    EmailConfirmationExpired = 1301_50_02,
    EmailConfirmationNotValid = 1301_50_01,
    CouldNotSentEmailConfirmation = 1301_51_01,


    PasswordResetEmailNotValid = 1302_50_01,
    PasswordResetEmailExpired = 1302_50_02,
    PasswordResetEmailIsUsed = 1302_41_01,
    CouldNotSentResetPasswordEmail = 1302_51_01,

    PasswordResetTokenNotValid = 1400_50_01,
    PasswordResetTokenExpired = 1400_50_02,
    PasswordResetTokenIsUsed = 1400_41_01,


    CannotParseIdsInUrlBadValue = 2000_30_02,
    CannotParseIdsInUrlTooManyIds = 2000_30_04,
    CannotParseIdsInUrlNonPositiveId = 2000_30_10,


    NotFound = 2000_40_01,
    Forbidden = 2000_50_10,
    CompanyNotFound = 2010_40_01,
    GenreNotFound = 2011_40_01,
    GameModeNotFound = 2012_40_01,
    PlatformNotFound = 2013_40_01,
    PlayerPerspectiveNotFound = 2014_40_01,
}