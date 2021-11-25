using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.GameMode;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.GameMode;
using YGL.API.Domain;
using YGL.API.Services.IControllers;

namespace YGL.API.Controllers.V1 {
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GameModeController : ControllerBase {
    private readonly IGameModeService _gameModeService;

    public GameModeController(IGameModeService gameModeService) {
        _gameModeService = gameModeService;
    }


    [HttpGet(Routes.GameMode.GetGameMode)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetGameModes(string gameModeIds) {
        IResponse res;

        GameModeResult gameModeResult = await _gameModeService.GetGameModes(gameModeIds);

        if (gameModeResult.IsSuccess) {
            res = gameModeResult.GameModes
                .Select(gm => new GameModeGetSuccRes() { GameMode = gm })
                .ToResponse(gameModeResult.GameModes.Count);

            return this.ReturnResult(gameModeResult.StatusCode, res);
        }

        res = new GameModeGetFailRes() {
            ErrorCodes = gameModeResult.ErrorCodes,
            ErrorMessages = gameModeResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(gameModeResult.StatusCode, res);
    }


    [HttpGet(Routes.GameMode.GetGameModes)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetGameModesFilter(
        [FromQuery] GameModeFilterQuery gameModeFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        IResponse res;

        GameModeResult gameModeResult =
            await _gameModeService.GetGameModesFilter(gameModeFilterQuery, (PaginationFilter)paginationQuery);

        if (gameModeResult.IsSuccess) {
            res = gameModeResult.GameModes
                .Select(gm => new GameModeGetSuccRes() { GameMode = gm })
                .ToPagedResponse(gameModeResult.GameModes.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(gameModeResult.StatusCode, res);
        }

        res = new GameModeGetFailRes() {
            ErrorCodes = gameModeResult.ErrorCodes,
            ErrorMessages = gameModeResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(gameModeResult.StatusCode, res);
    }
}
}