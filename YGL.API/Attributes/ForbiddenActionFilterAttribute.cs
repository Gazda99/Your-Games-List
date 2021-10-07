using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Errors;

namespace YGL.API.Attributes {
public class ForbiddenActionFilterAttribute : ActionFilterAttribute {
    public static void ReturnForbidden(ActionExecutingContext context) {
        const HttpStatusCode statusCode = HttpStatusCode.Forbidden;
        GeneralForbiddenForRes forbiddenForRes = new GeneralForbiddenForRes();
        forbiddenForRes.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.Forbidden);
        ResponseWithError<GeneralForbiddenForRes> forbiddenRes = forbiddenForRes.ToResponseWithErrors();
        context.Result = ControllerBaseExtensions.CreateActionResult(statusCode, forbiddenRes);
    }
}
}