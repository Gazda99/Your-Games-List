using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Genre;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers {
public interface IGenreService {
    public Task<GenreResult> GetGenres(string genreIds);
    public Task<GenreResult> GetGenresFilter(GenreFilterQuery genreFilterQuery, PaginationFilter paginationFilter);
}
}