﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.PlayerPerspective;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.PlayerPerspective;
using YGL.API.Domain;
using YGL.API.Services.Controllers;

namespace YGL.API.Controllers.V1 {
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PlayerPerspectiveController : ControllerBase {
    private readonly IPlayerPerspectiveService _perspectiveService;

    public PlayerPerspectiveController(IPlayerPerspectiveService perspectiveService) {
        _perspectiveService = perspectiveService;
    }


    [HttpGet(Routes.PlayerPerspective.GetPlayerPerspective)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetPlayerPerspective(int playerPerspectiveId) {
        IResponse res;

        PlayerPerspectiveResult perspectiveResult =
            await _perspectiveService.GetPlayerPerspective(playerPerspectiveId);

        if (perspectiveResult.IsSuccess) {
            res = perspectiveResult.PlayerPerspectives
                .Select(p => new PlayerPerspectiveGetSuccRes() { PlayerPerspective = p })
                .ToResponse();

            return this.ReturnResult(perspectiveResult.StatusCode, res);
        }

        res = new PlayerPerspectiveGetFailRes() {
            ErrorCodes = perspectiveResult.ErrorCodes,
            ErrorMessages = perspectiveResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(perspectiveResult.StatusCode, res);
    }


    [HttpGet(Routes.PlayerPerspective.GetPlayerPerspectives)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetPlayerPerspectives(
        [FromQuery] PlayerPerspectiveFilterQuery perspectiveFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        IResponse res;

        PlayerPerspectiveResult perspectiveResult =
            await _perspectiveService.GetPlayerPerspectives(perspectiveFilterQuery, (PaginationFilter)paginationQuery);

        if (perspectiveResult.IsSuccess) {
            res = perspectiveResult.PlayerPerspectives
                .Select(p => new PlayerPerspectiveGetSuccRes() { PlayerPerspective = p })
                .ToPagedResponse(perspectiveResult.PlayerPerspectives.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(perspectiveResult.StatusCode, res);
        }

        res = new PlayerPerspectiveGetFailRes() {
            ErrorCodes = perspectiveResult.ErrorCodes,
            ErrorMessages = perspectiveResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(perspectiveResult.StatusCode, res);
    }
}
}