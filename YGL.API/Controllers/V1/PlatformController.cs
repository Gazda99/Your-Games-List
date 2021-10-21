using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.Platform;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.Platform;
using YGL.API.Domain;
using YGL.API.Services.Controllers;

namespace YGL.API.Controllers.V1 {
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PlatformController : ControllerBase {
    private readonly IPlatformService _platformService;

    public PlatformController(IPlatformService platformService) {
        _platformService = platformService;
    }


    [HttpGet(Routes.Platform.GetPlatform)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetPlatform(int platformId) {
        IResponse res;

        PlatformResult platformResult =
            await _platformService.GetPlatform(platformId);

        if (platformResult.IsSuccess) {
            res = platformResult.Platforms
                .Select(p => new PlatformGetSuccRes() { Platform = p })
                .ToResponse();

            return this.ReturnResult(platformResult.StatusCode, res);
        }

        res = new PlatformGetFailRes() {
            ErrorCodes = platformResult.ErrorCodes,
            ErrorMessages = platformResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(platformResult.StatusCode, res);
    }


    [HttpGet(Routes.Platform.GetPlatforms)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetPlatforms(
        [FromQuery] PlatformFilterQuery platformFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        IResponse res;

        PlatformResult platformResult =
            await _platformService.GetPlatforms(platformFilterQuery, (PaginationFilter)paginationQuery);

        if (platformResult.IsSuccess) {
            res = platformResult.Platforms
                .Select(p => new PlatformGetSuccRes() { Platform = p })
                .ToPagedResponse(platformResult.Platforms.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(platformResult.StatusCode, res);
        }

        res = new PlatformGetFailRes() {
            ErrorCodes = platformResult.ErrorCodes,
            ErrorMessages = platformResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(platformResult.StatusCode, res);
    }
}
}