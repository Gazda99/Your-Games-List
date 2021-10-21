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

namespace YGL.API.Services.Controllers {
public class PlatformService : IPlatformService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public PlatformService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<PlatformResult> GetPlatform(int platformId) {
        PlatformResult platformResult = new PlatformResult();

        YGL.Model.Platform foundPlatform =
            await _yglDataContext.Platforms.FirstOrDefaultAsync(p => p.Id == platformId && p.ItemStatus == true);

        if (foundPlatform is null) {
            platformResult.IsSuccess = false;
            platformResult.StatusCode = HttpStatusCode.NotFound;
            platformResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlatformNotFound);
            return platformResult;
        }

        platformResult.Platforms = new List<SafePlatform>() { new SafePlatform(foundPlatform) };

        platformResult.IsSuccess = true;
        platformResult.StatusCode = HttpStatusCode.OK;
        return platformResult;
    }

    public async Task<PlatformResult> GetPlatforms(PlatformFilterQuery platformFilterQuery,
        PaginationFilter paginationFilter) {
        PlatformResult platformResult = new PlatformResult();

        IQueryable<YGL.Model.Platform> platformQueryable = _yglDataContext.Platforms
            .Where(p => p.ItemStatus == true);

        platformQueryable = AddFiltersOnQueryGetPlatforms(platformFilterQuery, platformQueryable);

        List<SafePlatform> safePlatforms =
            (await platformQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(p => new SafePlatform(p));

        platformResult.Platforms = safePlatforms;

        platformResult.IsSuccess = true;
        platformResult.StatusCode = HttpStatusCode.OK;
        return platformResult;
    }

    private static IQueryable<YGL.Model.Platform> AddFiltersOnQueryGetPlatforms(PlatformFilterQuery filterQuery,
        IQueryable<YGL.Model.Platform> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.PlatformName))
            queryable = queryable.Where(p => p.Name.Contains(filterQuery.PlatformName));

        return queryable;
    }
}
}