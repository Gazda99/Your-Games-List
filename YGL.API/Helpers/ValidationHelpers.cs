namespace YGL.API.Helpers {
public static class ValidationHelpers {
    public static bool CheckMinLength(string stringToCheck, int minLength) {
        return stringToCheck.Length >= minLength;
    }

    public static bool CheckMaxLength(string stringToCheck, int maxLength) {
        return stringToCheck.Length <= maxLength;
    }
}
}