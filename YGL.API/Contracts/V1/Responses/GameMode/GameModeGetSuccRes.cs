using YGL.API.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.GameMode; 

public class GameModeGetSuccRes : IObjectForResponse {
    public SafeGameMode GameMode { get; set; }
}