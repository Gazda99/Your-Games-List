using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.Platform;
using YGL.API.Domain;
using YGL.API.Errors;
using YGL.API.SafeObjects;
using YGL.API.Services.IControllers;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers;

public class PlatformService : IPlatformService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public PlatformService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<PlatformResult> GetPlatforms(string platformIds) {
        var platformResult = new PlatformResult();

        if (!ValidationUrl.TryParseInt(platformIds, platformResult, out List<int> ids)) {
            platformResult.IsSuccess = false;
            platformResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return platformResult;
        }

        var foundPlatforms = await _yglDataContext.Platforms.Where(p => ids.Contains(p.Id)).ToListAsync();


        if (foundPlatforms is null || foundPlatforms.Count == 0) {
            platformResult.IsSuccess = false;
            platformResult.StatusCode = HttpStatusCode.NotFound;
            platformResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlatformNotFound);
            return platformResult;
        }

        platformResult.Platforms = foundPlatforms.ConvertAll(p => new SafePlatform(p));

        platformResult.IsSuccess = true;
        platformResult.StatusCode = HttpStatusCode.OK;
        return platformResult;
    }

    public async Task<PlatformResult> GetPlatformsFilter(PlatformFilterQuery platformFilterQuery,
        PaginationFilter paginationFilter) {
        var platformResult = new PlatformResult();

        var platformQueryable = _yglDataContext.Platforms.Where(p => p.ItemStatus == true);

        platformQueryable = AddFiltersOnQueryGetPlatforms(platformFilterQuery, platformQueryable);

        var foundPlatforms = (await platformQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take));

        if (foundPlatforms is null || foundPlatforms.Count == 0) {
            platformResult.IsSuccess = false;
            platformResult.StatusCode = HttpStatusCode.NotFound;
            platformResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlatformNotFound);
            return platformResult;
        }

        platformResult.Platforms = foundPlatforms.ConvertAll(p => new SafePlatform(p));

        platformResult.IsSuccess = true;
        platformResult.StatusCode = HttpStatusCode.OK;
        return platformResult;
    }

    private static IQueryable<YGL.Model.Platform> AddFiltersOnQueryGetPlatforms(PlatformFilterQuery filterQuery,
        IQueryable<YGL.Model.Platform> queryable) {
        if (filterQuery is null) return queryable;

        if (!string.IsNullOrEmpty(filterQuery.PlatformName))
            queryable = queryable.Where(p => p.Name.Contains(filterQuery.PlatformName));

        return queryable;
    }
}