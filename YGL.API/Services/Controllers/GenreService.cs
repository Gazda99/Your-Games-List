using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.Genre;
using YGL.API.Domain;
using YGL.API.Errors;
using YGL.API.SafeObjects;
using YGL.API.Services.IControllers;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers {
public class GenreService : IGenreService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public GenreService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<GenreResult> GetGenres(string genreIds) {
        GenreResult genreResult = new GenreResult();

        if (!ValidationUrl.TryParseInt(genreIds, genreResult, out List<int> ids)) {
            genreResult.IsSuccess = false;
            genreResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return genreResult;
        }

        List<YGL.Model.Genre> foundGenres = await _yglDataContext.Genres.Where(g => ids.Contains(g.Id)).ToListAsync();

        if (foundGenres is null || foundGenres.Count == 0) {
            genreResult.IsSuccess = false;
            genreResult.StatusCode = HttpStatusCode.NotFound;
            genreResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.GenreNotFound);
            return genreResult;
        }

        genreResult.Genres = foundGenres.ConvertAll(g => new SafeGenre(g));

        genreResult.IsSuccess = true;
        genreResult.StatusCode = HttpStatusCode.OK;
        return genreResult;
    }

    public async Task<GenreResult> GetGenresFilter(GenreFilterQuery genreFilterQuery, PaginationFilter paginationFilter) {
        GenreResult genreResult = new GenreResult();

        IQueryable<YGL.Model.Genre> genreQueryable = _yglDataContext.Genres
            .Where(g => g.ItemStatus == true);

        genreQueryable = AddFiltersOnQueryGetGenres(genreFilterQuery, genreQueryable);

        List<YGL.Model.Genre> foundGenres =
            (await genreQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take));

        if (foundGenres is null || foundGenres.Count == 0) {
            genreResult.IsSuccess = false;
            genreResult.StatusCode = HttpStatusCode.NotFound;
            genreResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CompanyNotFound);
            return genreResult;
        }

        genreResult.Genres = foundGenres.ConvertAll(g => new SafeGenre(g));

        genreResult.IsSuccess = true;
        genreResult.StatusCode = HttpStatusCode.OK;
        return genreResult;
    }

    private static IQueryable<YGL.Model.Genre> AddFiltersOnQueryGetGenres(GenreFilterQuery filterQuery,
        IQueryable<YGL.Model.Genre> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.GenreName))
            queryable = queryable.Where(g => g.Name.Contains(filterQuery.GenreName));

        return queryable;
    }
}
}