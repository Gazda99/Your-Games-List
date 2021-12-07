using System;
using System.Text.RegularExpressions;
using YGL.API.EnumTypes;
using YGL.API.Errors;
using YGL.API.Helpers;

namespace YGL.API.Validation; 

public static class ValidationUser {
    private const int PasswordMinLength = 8;
    private const int PasswordMaxLength = 128;

    private const int UsernameMinLength = 6;
    private const int UsernameMaxLength = 50;

    private const int EmailMaxLength = 255;

    private const int UserAboutMaxLength = 2000;

    private const int UserSlugMinLength = UsernameMinLength;
    private const int UserSlugMaxLength = UsernameMaxLength;

    private const int BirthYearMin = 1900;
    private static readonly int BirthYearMax = DateTime.UtcNow.Year;


    private static readonly Regex UsernameRegex = new Regex(@"^[A-Za-z0-9]+(?:[_][A-Za-z0-9]+)*$");

    public static bool ValidateEmail(string email, IErrorList errorList) {
        if (!ValidationHelpers.CheckMaxLength(email, EmailMaxLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.EmailIsTooLong);

            return false;
        }

        try {
            System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(email);
            var check = mailAddress.Address == email;

            if (!check)
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(
                    ApiErrorCodes.EmailDoesNotMatchCriteria);

            return check;
        }
        catch {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.EmailDoesNotMatchCriteria);
            return false;
        }
    }

    public static bool ValidateUsername(string username, IErrorList errorList) {
        if (String.IsNullOrEmpty(username) || (!ValidationHelpers.CheckMinLength(username, UsernameMinLength))) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameIsTooShort);
            return false;
        }

        if (!ValidationHelpers.CheckMaxLength(username, UsernameMaxLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameIsTooLong);
            return false;
        }

        //check if username consists only of allowed characters and match allowed pattern
        if (!UsernameRegex.IsMatch(username)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UsernameDoesNotMatchCriteria);
            return false;
        }

        return true;
    }

    public static bool ValidatePassword(string password, IErrorList errorList) {
        if (String.IsNullOrEmpty(password) || !ValidationHelpers.CheckMinLength(password, PasswordMinLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PasswordIsTooShort);
            return false;
        }

        if (!ValidationHelpers.CheckMaxLength(password, PasswordMaxLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PasswordIsTooLong);
            return false;
        }

        return true;
    }

    public static bool ValidateUserAbout(string about, IErrorList errorList) {
        if (!ValidationHelpers.CheckMaxLength(about, UserAboutMaxLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .AboutIsTooLong);
            return false;
        }

        return true;
    }

    public static bool ValidateSlug(string slug, IErrorList errorList) {
        if (String.IsNullOrEmpty(slug)) {
            return true;
        }

        if (!ValidationHelpers.CheckMinLength(slug, UserSlugMinLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.SlugIsTooShort);
            return false;
        }

        if (!ValidationHelpers.CheckMaxLength(slug, UserSlugMaxLength)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes
                .SlugIsTooLong);
            return false;
        }

        return true;
    }

    public static bool ValidateBirthYear(short birthYear, IErrorList errorList) {
        if (birthYear == -1)
            return true;

        if (birthYear < BirthYearMin) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.BirthYearTooLow);
            return false;
        }

        if (birthYear > BirthYearMax) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.BirthYearTooHigh);
            return false;
        }

        return true;
    }

    public static bool ValidateCountry(short countryCode, IErrorList errorList) {
        if (countryCode == (short)Country.Default) {
            return true;
        }

        if (!Country.DoesExists(countryCode)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.InvalidCountry);
            return false;
        }

        return true;
    }

    public static bool ValidateGender(byte gender, IErrorList errorList) {
        if (gender == (byte)Gender.Default) {
            return true;
        }

        if (!Gender.DoesExists(gender)) {
            errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.InvalidGender);
            return false;
        }

        return true;
    }
}