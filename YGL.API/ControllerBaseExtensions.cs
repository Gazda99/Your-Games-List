using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Contracts.V1.Responses;

namespace YGL.API; 

public static class ControllerBaseExtensions {
    private const string RouteNotDefined = "Route not defined";

    public static IActionResult ReturnResult(this ControllerBase _, HttpStatusCode httpStatusCode, IResponse returnObject,
        string route = RouteNotDefined) {
        return GetResult(httpStatusCode, returnObject, route);
    }

    public static IActionResult CreateActionResult(HttpStatusCode httpStatusCode, IResponse returnObject,
        string route = RouteNotDefined) {
        return GetResult(httpStatusCode, returnObject, route);
    }

    private static IActionResult GetResult(HttpStatusCode statusCode, IResponse returnObject,
        string route = RouteNotDefined) {
        return statusCode switch {
            HttpStatusCode.OK => new OkObjectResult(returnObject),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(returnObject),
            HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(returnObject),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(returnObject),
            HttpStatusCode.Created => new CreatedAtRouteResult(route, returnObject),
            HttpStatusCode.Conflict => new ConflictObjectResult(returnObject),
            HttpStatusCode.NotFound => new NotFoundObjectResult(returnObject),

            HttpStatusCode.Forbidden => new CustomObjectResult(returnObject, HttpStatusCode.Forbidden),
            HttpStatusCode.InternalServerError => new CustomObjectResult(returnObject,
                HttpStatusCode.InternalServerError),
            // HttpStatusCode.Continue => Con,
            // HttpStatusCode.SwitchingProtocols => expr,
            // HttpStatusCode.Processing => expr,
            // HttpStatusCode.EarlyHints => expr,
            // HttpStatusCode.Accepted => new AcceptedResult(),
            // HttpStatusCode.NonAuthoritativeInformation => expr,
            // HttpStatusCode.NoContent => new NoContentResult(),
            // HttpStatusCode.ResetContent => expr,
            // HttpStatusCode.PartialContent => expr,
            // HttpStatusCode.MultiStatus => expr,
            // HttpStatusCode.AlreadyReported => expr,
            // HttpStatusCode.IMUsed => expr,
            // HttpStatusCode.Ambiguous => expr,
            // HttpStatusCode.Moved => expr,
            // HttpStatusCode.Found => expr,
            // HttpStatusCode.RedirectMethod => expr,
            // HttpStatusCode.NotModified => expr,
            // HttpStatusCode.UseProxy => expr,
            // HttpStatusCode.Unused => expr,
            // HttpStatusCode.RedirectKeepVerb => expr,
            // HttpStatusCode.PermanentRedirect => expr,
            // HttpStatusCode.PaymentRequired => expr,
            // HttpStatusCode.MethodNotAllowed => expr,
            // HttpStatusCode.NotAcceptable => expr,
            // HttpStatusCode.ProxyAuthenticationRequired => expr,
            // HttpStatusCode.RequestTimeout => expr,
            // HttpStatusCode.Gone => expr,
            // HttpStatusCode.LengthRequired => expr,
            // HttpStatusCode.PreconditionFailed => expr,
            // HttpStatusCode.RequestEntityTooLarge => expr,
            // HttpStatusCode.RequestUriTooLong => expr,
            // HttpStatusCode.UnsupportedMediaType => expr,
            // HttpStatusCode.RequestedRangeNotSatisfiable => expr,
            // HttpStatusCode.ExpectationFailed => expr,
            // HttpStatusCode.MisdirectedRequest => expr,
            // HttpStatusCode.Locked => expr,
            // HttpStatusCode.FailedDependency => expr,
            // HttpStatusCode.UpgradeRequired => expr,
            // HttpStatusCode.PreconditionRequired => expr,
            // HttpStatusCode.TooManyRequests => expr,
            // HttpStatusCode.RequestHeaderFieldsTooLarge => expr,
            // HttpStatusCode.UnavailableForLegalReasons => expr,
            // HttpStatusCode.NotImplemented => expr,
            // HttpStatusCode.BadGateway => expr,
            // HttpStatusCode.ServiceUnavailable => expr,
            // HttpStatusCode.GatewayTimeout => expr,
            // HttpStatusCode.HttpVersionNotSupported => expr,
            // HttpStatusCode.VariantAlsoNegotiates => expr,
            // HttpStatusCode.InsufficientStorage => expr,
            // HttpStatusCode.LoopDetected => expr,
            // HttpStatusCode.NotExtended => expr,
            // HttpStatusCode.NetworkAuthenticationRequired => expr,
            _ => throw new NotSupportedException()
        };
    }
}

public class CustomObjectResult : ObjectResult {
    public CustomObjectResult(IResponse value, HttpStatusCode statusCode) : base(value) {
        base.StatusCode = (int)statusCode;
    }

    public CustomObjectResult(IResponse value, int statusCode) : base(value) {
        base.StatusCode = statusCode;
    }
}