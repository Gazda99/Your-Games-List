using YGL.API.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.Platform {
public class PlatformGetSuccRes : IObjectForResponse {
    public SafePlatform Platform { get; set; }
}
}