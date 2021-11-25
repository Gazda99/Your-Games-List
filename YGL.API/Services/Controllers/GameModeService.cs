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
using YGL.API.Services.IControllers;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers {
public class GameModeService : IGameModeService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public GameModeService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<GameModeResult> GetGameModes(string gameModesIds) {
        GameModeResult gameModeResult = new GameModeResult();

        if (!ValidationUrl.TryParseInt(gameModesIds, gameModeResult, out List<int> ids)) {
            gameModeResult.IsSuccess = false;
            gameModeResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return gameModeResult;
        }

        List<YGL.Model.GameMode> foundGameModes = await _yglDataContext.GameModes.Where(gm => ids.Contains(gm.Id)).ToListAsync();

        if (foundGameModes is null || foundGameModes.Count == 0) {
            gameModeResult.IsSuccess = false;
            gameModeResult.StatusCode = HttpStatusCode.NotFound;
            gameModeResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.GameModeNotFound);
            return gameModeResult;
        }

        gameModeResult.GameModes = foundGameModes.ConvertAll(gm => new SafeGameMode(gm));

        gameModeResult.IsSuccess = true;
        gameModeResult.StatusCode = HttpStatusCode.OK;
        return gameModeResult;
    }

    public async Task<GameModeResult> GetGameModesFilter(GameModeFilterQuery gameModeFilterQuery,
        PaginationFilter paginationFilter) {
        GameModeResult gameModeResult = new GameModeResult();

        IQueryable<YGL.Model.GameMode> gameModeQueryable = _yglDataContext.GameModes
            .Where(gm => gm.ItemStatus == true);

        gameModeQueryable = AddFiltersOnQueryGetGameModes(gameModeFilterQuery, gameModeQueryable);

        List<YGL.Model.GameMode> foundGameModes =
            (await gameModeQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take));

        if (foundGameModes is null || foundGameModes.Count == 0) {
            gameModeResult.IsSuccess = false;
            gameModeResult.StatusCode = HttpStatusCode.NotFound;
            gameModeResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.GameModeNotFound);
            return gameModeResult;
        }

        gameModeResult.GameModes = foundGameModes.ConvertAll(gm => new SafeGameMode(gm));
        
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