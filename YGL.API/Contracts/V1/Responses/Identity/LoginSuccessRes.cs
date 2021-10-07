namespace YGL.API.Contracts.V1.Responses.Identity {
public class LoginSuccessRes : IObjectForResponse {
    public long UserId { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}
}