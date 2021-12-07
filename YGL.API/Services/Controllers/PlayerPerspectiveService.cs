using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.PlayerPerspective;
using YGL.API.Domain;
using YGL.API.Errors;
using YGL.API.SafeObjects;
using YGL.API.Services.IControllers;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers;

public class PlayerPerspectiveService : IPlayerPerspectiveService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public PlayerPerspectiveService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<PlayerPerspectiveResult> GetPlayerPerspectives(string perspectiveIds) {
        var perspectiveResult = new PlayerPerspectiveResult();

        if (!ValidationUrl.TryParseInt(perspectiveIds, perspectiveResult, out List<int> ids)) {
            perspectiveResult.IsSuccess = false;
            perspectiveResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return perspectiveResult;
        }

        var foundPerspectives = await _yglDataContext.PlayerPerspectives.Where(p => ids.Contains(p.Id)).ToListAsync();

        if (foundPerspectives is null || foundPerspectives.Count == 0) {
            perspectiveResult.IsSuccess = false;
            perspectiveResult.StatusCode = HttpStatusCode.NotFound;
            perspectiveResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlayerPerspectiveNotFound);
            return perspectiveResult;
        }

        perspectiveResult.PlayerPerspectives = foundPerspectives.ConvertAll(p => new SafePlayerPerspective(p));

        perspectiveResult.IsSuccess = true;
        perspectiveResult.StatusCode = HttpStatusCode.OK;
        return perspectiveResult;
    }

    public async Task<PlayerPerspectiveResult> GetPlayerPerspectivesFilter(PlayerPerspectiveFilterQuery platformFilterQuery,
        PaginationFilter paginationFilter) {
        var perspectiveResult = new PlayerPerspectiveResult();

        var perspectivesQueryable = _yglDataContext.PlayerPerspectives.Where(u => u.ItemStatus == true);

        perspectivesQueryable = AddFiltersOnQueryGetPlayerPerspectives(platformFilterQuery, perspectivesQueryable);

        var foundPerspectives = (await perspectivesQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take));

        if (foundPerspectives is null || foundPerspectives.Count == 0) {
            perspectiveResult.IsSuccess = false;
            perspectiveResult.StatusCode = HttpStatusCode.NotFound;
            perspectiveResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlayerPerspectiveNotFound);
            return perspectiveResult;
        }

        perspectiveResult.PlayerPerspectives = foundPerspectives.ConvertAll(g => new SafePlayerPerspective(g));

        perspectiveResult.IsSuccess = true;
        perspectiveResult.StatusCode = HttpStatusCode.OK;
        return perspectiveResult;
    }

    private static IQueryable<YGL.Model.PlayerPerspective> AddFiltersOnQueryGetPlayerPerspectives(
        PlayerPerspectiveFilterQuery filterQuery,
        IQueryable<YGL.Model.PlayerPerspective> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.PlayerPerspectiveName))
            queryable = queryable.Where(p => p.Name.Contains(filterQuery.PlayerPerspectiveName));

        return queryable;
    }
}