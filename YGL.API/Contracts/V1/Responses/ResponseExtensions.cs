using System.Collections.Generic;
using System.Linq;
using YGL.API.Domain;
using YGL.API.Errors;

namespace YGL.API.Contracts.V1.Responses; 

public static class ResponseExtensions {
    public static SingleResponse<T> ToSingleResponse<T>(this T response) where T : IObjectForResponse {
        return new SingleResponse<T>(response);
    }

    public static SingleResponse<T> ToSingleResponse<T>(this IEnumerable<T> response) where T : IObjectForResponse {
        return ToSingleResponse(response.FirstOrDefault());
    }

    public static Response<T> ToResponse<T>(this IEnumerable<T> response, int count) where T : IObjectForResponse {
        return new Response<T>(response) {
            Amount = count
        };
    }


    public static PagedResponse<T> ToPagedResponse<T>(this IEnumerable<T> response, int count, PaginationFilter paginationFilter)
        where T : IObjectForResponse {
        return new PagedResponse<T>(response) {
            Amount = count,
            Skipped = paginationFilter.Skip,
            NextAt = paginationFilter.Skip + paginationFilter.Take
        };
    }

    public static ResponseWithError<T> ToResponseWithErrors<T>(this T response) where T : IObjectForResponseWithErrors {
        return new ResponseWithError<T>(response);
    }

    public static IObjectForResponseWithErrors WithErrors(this IObjectForResponseWithErrors thisErrorLists, IErrorList errorList) {
        thisErrorLists.ErrorCodes = errorList.ErrorCodes;
        thisErrorLists.ErrorMessages = errorList.ErrorMessages;
        return thisErrorLists;
    }
}