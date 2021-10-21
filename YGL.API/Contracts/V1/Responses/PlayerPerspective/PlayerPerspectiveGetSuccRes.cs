using YGL.API.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.PlayerPerspective {
public class PlayerPerspectiveGetSuccRes : IObjectForResponse {
    public SafePlayerPerspective PlayerPerspective { get; set; }
}
}