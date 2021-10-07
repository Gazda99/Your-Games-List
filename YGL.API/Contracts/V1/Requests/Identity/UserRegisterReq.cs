using System.ComponentModel.DataAnnotations;

namespace YGL.API.Contracts.V1.Requests.Identity {
public class UserRegisterReq {
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}
}