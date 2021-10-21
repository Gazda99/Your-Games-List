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

namespace YGL.API.Services.Controllers {
public class GenreService : IGenreService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public GenreService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<GenreResult> GetGenre(int genreId) {
        GenreResult genreResult = new GenreResult();

        YGL.Model.Genre foundGenre =
            await _yglDataContext.Genres.FirstOrDefaultAsync(g => g.Id == genreId && g.ItemStatus == true);

        if (foundGenre is null) {
            genreResult.IsSuccess = false;
            genreResult.StatusCode = HttpStatusCode.NotFound;
            genreResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.GenreNotFound);
            return genreResult;
        }

        genreResult.Genres = new List<SafeGenre>() { new SafeGenre(foundGenre) };

        genreResult.IsSuccess = true;
        genreResult.StatusCode = HttpStatusCode.OK;
        return genreResult;
    }

    public async Task<GenreResult> GetGenres(GenreFilterQuery genreFilterQuery, PaginationFilter paginationFilter) {
        GenreResult genreResult = new GenreResult();

        IQueryable<YGL.Model.Genre> genreQueryable = _yglDataContext.Genres
            .Where(g => g.ItemStatus == true);

        genreQueryable = AddFiltersOnQueryGetGenres(genreFilterQuery, genreQueryable);

        List<SafeGenre> safeGenres =
            (await genreQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(g => new SafeGenre(g));

        genreResult.Genres = safeGenres;

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