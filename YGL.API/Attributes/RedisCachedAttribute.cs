using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using YGL.API.Domain;
using YGL.API.Services;
using YGL.API.Settings;

namespace YGL.API.Attributes {
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RedisCachedAttribute : Attribute, IAsyncActionFilter {
    private readonly TimeSpan _timeToLiveSeconds;

    public RedisCachedAttribute(int timeToLiveSeconds) {
        _timeToLiveSeconds = TimeSpan.FromSeconds(timeToLiveSeconds);
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        //before
        RedisCacheSettings cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
        Console.WriteLine();

        if (!cacheSettings.Enabled) {
            await next();
            return;
        }

        IRedisCacheService cacheService =
            context.HttpContext.RequestServices.GetRequiredService<IRedisCacheService>();

        string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        string cachedResponse;

        try {
            cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
        }
        catch (StackExchange.Redis.RedisConnectionException) {
            await next();
            return;
        }

        if (!String.IsNullOrEmpty(cachedResponse)) {
            context.Result = new OkObjectResult(cachedResponse);
            return;
        }

        ActionExecutedContext executedContext = await next();

        //after
        ObjectResult response = (ObjectResult)executedContext.Result;
        object responseBody = response.Value;


        if (CheckStatusCode(response.StatusCode)) {
            try {
                await cacheService.SetCachedResponseAsync(cacheKey, responseBody, _timeToLiveSeconds);
            }
            catch (StackExchange.Redis.RedisConnectionException) {
                return;
            }
        }
    }


    private static string GenerateCacheKeyFromRequest(HttpRequest request) {
        const string skipKey = "skip";
        const string takeKey = "take";

        StringBuilder sb = new StringBuilder();
        sb.Append(request.Path.ToString());

        Dictionary<string, StringValues> queryParams = request.Query
            .ToDictionary(q => q.Key, q => q.Value);

        QueryString queryString = request.QueryString;

        bool check = false;
        if (request.Query.ContainsKey(skipKey)) {
            int skip = GetValueFromQuery(request.Query, skipKey);
            skip = PaginationFilter.SetSkip(skip);
            queryParams[skipKey] = skip.ToString();
            check = true;
        }

        if (request.Query.ContainsKey(takeKey)) {
            int take = GetValueFromQuery(request.Query, takeKey);
            take = PaginationFilter.SetTake(take);
            queryParams[takeKey] = take.ToString();
            check = true;
        }

        if (check) {
            IQueryCollection changedQueryCollection = new QueryCollection(queryParams);
            queryString = QueryString.Create(changedQueryCollection);
        }

        sb.Append(queryString);
        Console.WriteLine(sb.ToString());
        return sb.ToString();
    }

    private static int GetValueFromQuery(IQueryCollection query, string key) {
        Int32.TryParse(query[key].ToString(), out int value);
        return value;
    }

    private static bool CheckStatusCode(int? statusCode) {
        const int ok = (int)HttpStatusCode.OK;
        return statusCode == ok;
    }
}
}