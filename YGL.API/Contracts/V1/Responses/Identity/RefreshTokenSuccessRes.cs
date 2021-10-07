namespace YGL.API.Contracts.V1.Responses.Identity {
public class RefreshTokenSuccessRes : IObjectForResponse {
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}
}