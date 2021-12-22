using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YGL.API.Contracts;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Errors;

namespace YGL.API.Services;

public static class ExceptionHandlerService {
    public static void HandleException(IApplicationBuilder app) => app.Run(Handler);

    private static async Task Handler(HttpContext context) {
        try {
            Exception exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            if (exception.Message.Contains("connection to SQL Server")) {
                var res = new DbConnectionErrorRes();
                res.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.DbConnectionError);

                context.Response.ContentType = ContentTypes.ApplicationJson;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
            }
            else {
                await UnspecifiedError(context);
            }
        }
        catch (Exception) {
            await UnspecifiedError(context);
        }
    }


    private static async Task UnspecifiedError(HttpContext context) {
        var res = new GeneralErrorResponse();
        res.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UnspecifiedError);

        context.Response.ContentType = ContentTypes.ApplicationJson;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
    }
}