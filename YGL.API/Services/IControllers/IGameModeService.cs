using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.GameMode;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers; 

public interface IGameModeService {
    public Task<GameModeResult> GetGameModes(string gameModesIds);

    public Task<GameModeResult> GetGameModesFilter(GameModeFilterQuery gameModeFilterQuery, PaginationFilter paginationFilter);
}