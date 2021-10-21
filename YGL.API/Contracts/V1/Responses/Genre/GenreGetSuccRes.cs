using YGL.API.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.Genre {
public class GenreGetSuccRes  : IObjectForResponse {
    public SafeGenre Genre { get; set; }
}
}