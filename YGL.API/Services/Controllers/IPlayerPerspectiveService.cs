using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.PlayerPerspective;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface IPlayerPerspectiveService {
    public Task<PlayerPerspectiveResult> GetPlayerPerspective(int perspectiveId);

    public Task<PlayerPerspectiveResult> GetPlayerPerspectives(PlayerPerspectiveFilterQuery playerPerspectiveFilterQuery,
        PaginationFilter paginationFilter);
}
}