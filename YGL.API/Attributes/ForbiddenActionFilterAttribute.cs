using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Errors;

namespace YGL.API.Attributes; 

public class ForbiddenActionFilterAttribute : ActionFilterAttribute {
    public static void ReturnForbidden(ActionExecutingContext context) {
        const HttpStatusCode statusCode = HttpStatusCode.Forbidden;
        var forbiddenForRes = new GeneralErrorResponse();
        forbiddenForRes.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.Forbidden);
        ResponseWithError<GeneralErrorResponse> forbiddenRes = forbiddenForRes.ToResponseWithErrors();
        context.Result = ControllerBaseExtensions.CreateActionResult(statusCode, forbiddenRes);
    }
}