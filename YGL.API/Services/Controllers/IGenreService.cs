using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Genre;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface IGenreService {
    public Task<GenreResult> GetGenre(int genreId);
    public Task<GenreResult> GetGenres(GenreFilterQuery genreFilterQuery, PaginationFilter paginationFilter);
}
}