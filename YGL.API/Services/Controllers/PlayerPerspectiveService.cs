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

namespace YGL.API.Services.Controllers {
public class PlayerPerspectiveService : IPlayerPerspectiveService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public PlayerPerspectiveService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<PlayerPerspectiveResult> GetPlayerPerspective(int perspectiveId) {
        PlayerPerspectiveResult perspectiveResult = new PlayerPerspectiveResult();

        YGL.Model.PlayerPerspective foundPerspective =
            await _yglDataContext.PlayerPerspectives.FirstOrDefaultAsync(p =>
                p.Id == perspectiveId && p.ItemStatus == true);

        if (foundPerspective is null) {
            perspectiveResult.IsSuccess = false;
            perspectiveResult.StatusCode = HttpStatusCode.NotFound;
            perspectiveResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.PlayerPerspectiveNotFound);
            return perspectiveResult;
        }

        perspectiveResult.PlayerPerspectives = new List<SafePlayerPerspective>()
            { new SafePlayerPerspective(foundPerspective) };

        perspectiveResult.IsSuccess = true;
        perspectiveResult.StatusCode = HttpStatusCode.OK;
        return perspectiveResult;
    }

    public async Task<PlayerPerspectiveResult> GetPlayerPerspectives(PlayerPerspectiveFilterQuery platformFilterQuery,
        PaginationFilter paginationFilter) {
        PlayerPerspectiveResult perspectiveResult = new PlayerPerspectiveResult();

        IQueryable<YGL.Model.PlayerPerspective> perspectivesQueryable = _yglDataContext.PlayerPerspectives
            .Where(u => u.ItemStatus == true);

        perspectivesQueryable = AddFiltersOnQueryGetPlayerPerspectives(platformFilterQuery, perspectivesQueryable);

        List<SafePlayerPerspective> safePlayerPerspectives =
            (await perspectivesQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(g => new SafePlayerPerspective(g));

        perspectiveResult.PlayerPerspectives = safePlayerPerspectives;

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
}