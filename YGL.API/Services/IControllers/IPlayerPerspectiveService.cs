using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.PlayerPerspective;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers {
public interface IPlayerPerspectiveService {
    public Task<PlayerPerspectiveResult> GetPlayerPerspectives(string perspectiveIds);

    public Task<PlayerPerspectiveResult> GetPlayerPerspectivesFilter(PlayerPerspectiveFilterQuery playerPerspectiveFilterQuery,
        PaginationFilter paginationFilter);
}
}