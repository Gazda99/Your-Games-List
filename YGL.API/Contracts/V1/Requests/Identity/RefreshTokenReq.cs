using System.ComponentModel.DataAnnotations;

namespace YGL.API.Contracts.V1.Requests.Identity {
public class RefreshTokenReq {
    [Required] public string JwtToken { get; set; }
    [Required] public string RefreshToken { get; set; }
}
}