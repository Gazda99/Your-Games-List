using System;
using System.Collections.Generic;
using YGL.API.Helpers;

namespace YGL.API.Errors {
public static class ApiErrorExtensions {
    private const string NoErrorDescription = "No error description.";

    private static void CheckForExistence(this IErrorList iErrorList) {
        iErrorList.ErrorCodes ??= new List<int>();
        iErrorList.ErrorMessages ??= new List<string>();
    }


    public static void AddErrors<T, TEnum>(this IErrorList iErrorList, params TEnum[] errorCodes)
        where T : IErrorContainer<TEnum> where TEnum : Enum {
        if (errorCodes is null) return;
        if (errorCodes.Length < 1) return;

        iErrorList.CheckForExistence();

        IErrorContainer<TEnum> iErrorContainer;

        try {
            iErrorContainer = Activator.CreateInstance<T>();
        }

        catch (Exception) {
            return;
        }

        foreach (TEnum errorCode in errorCodes) {
            iErrorList.ErrorCodes.Add(errorCode.ToInt());

            string errorMessage = iErrorContainer?.Errors is null || !iErrorContainer.Errors.ContainsKey(errorCode)
                ? NoErrorDescription
                : iErrorContainer.Errors[errorCode];

            iErrorList.ErrorMessages.Add(errorMessage);
        }
    }

    public static void CreateErrors(this IErrorList iErrorList, IErrorList iErrorList2) {
        iErrorList.ErrorCodes = iErrorList2.ErrorCodes;
        iErrorList.ErrorMessages = iErrorList2.ErrorMessages;
    }
}
}