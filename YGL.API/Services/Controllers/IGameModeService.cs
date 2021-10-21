using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.GameMode;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface IGameModeService {
    public Task<GameModeResult> GetGameMode(int companyId);

    public Task<GameModeResult> GetGameModes(GameModeFilterQuery gameModeFilterQuery,
        PaginationFilter paginationFilter);
}
}