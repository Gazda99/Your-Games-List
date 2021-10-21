using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.GameMode;
using YGL.API.Domain;
using YGL.API.Errors;
using YGL.API.SafeObjects;

namespace YGL.API.Services.Controllers {
public class GameModeService : IGameModeService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public GameModeService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<GameModeResult> GetGameMode(int gameModeId) {
        GameModeResult gameModeResult = new GameModeResult();

        YGL.Model.GameMode foundGameMode =
            await _yglDataContext.GameModes.FirstOrDefaultAsync(gm => gm.Id == gameModeId && gm.ItemStatus == true);

        if (foundGameMode is null) {
            gameModeResult.IsSuccess = false;
            gameModeResult.StatusCode = HttpStatusCode.NotFound;
            gameModeResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.GameModeNotFound);
            return gameModeResult;
        }

        gameModeResult.GameModes = new List<SafeGameMode>() { new SafeGameMode(foundGameMode) };

        gameModeResult.IsSuccess = true;
        gameModeResult.StatusCode = HttpStatusCode.OK;
        return gameModeResult;
    }

    public async Task<GameModeResult> GetGameModes(GameModeFilterQuery gameModeFilterQuery,
        PaginationFilter paginationFilter) {
        GameModeResult gameModeResult = new GameModeResult();

        IQueryable<YGL.Model.GameMode> gameModeQueryable = _yglDataContext.GameModes
            .Where(gm => gm.ItemStatus == true);

        gameModeQueryable = AddFiltersOnQueryGetGameModes(gameModeFilterQuery, gameModeQueryable);

        List<SafeGameMode> safeGameModes =
            (await gameModeQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(gm => new SafeGameMode(gm));

        gameModeResult.GameModes = safeGameModes;

        gameModeResult.IsSuccess = true;
        gameModeResult.StatusCode = HttpStatusCode.OK;
        return gameModeResult;
    }

    private static IQueryable<YGL.Model.GameMode> AddFiltersOnQueryGetGameModes(GameModeFilterQuery filterQuery,
        IQueryable<YGL.Model.GameMode> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.GameModeName))
            queryable = queryable.Where(gm => gm.Name.Contains(filterQuery.GameModeName));

        return queryable;
    }
}
}