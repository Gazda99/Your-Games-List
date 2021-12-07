using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.Genre;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.Genre;
using YGL.API.Domain;
using YGL.API.Services.IControllers;

namespace YGL.API.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GenreController : ControllerBase {
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService) {
        _genreService = genreService;
    }


    [HttpGet(Routes.Genre.GetGenre)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetGenres(string genreIds) {
        IResponse res;

        GenreResult genreResult = await _genreService.GetGenres(genreIds);

        if (genreResult.IsSuccess) {
            res = genreResult.Genres
                .Select(g => new GenreGetSuccRes() { Genre = g })
                .ToResponse(genreResult.Genres.Count);

            return this.ReturnResult(genreResult.StatusCode, res);
        }

        res = new GenreGetFailRes() {
            ErrorCodes = genreResult.ErrorCodes,
            ErrorMessages = genreResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(genreResult.StatusCode, res);
    }


    [HttpGet(Routes.Genre.GetGenres)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetGenresFilter(
        [FromQuery] GenreFilterQuery genreFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        IResponse res;

        GenreResult genreResult = await _genreService.GetGenresFilter(genreFilterQuery, (PaginationFilter)paginationQuery);

        if (genreResult.IsSuccess) {
            res = genreResult.Genres
                .Select(g => new GenreGetSuccRes() { Genre = g })
                .ToPagedResponse(genreResult.Genres.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(genreResult.StatusCode, res);
        }

        res = new GenreGetFailRes() {
            ErrorCodes = genreResult.ErrorCodes,
            ErrorMessages = genreResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(genreResult.StatusCode, res);
    }
}