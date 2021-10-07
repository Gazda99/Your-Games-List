using System.ComponentModel.DataAnnotations;

namespace YGL.API.Contracts.V1.Requests.Identity {
public class EmailConfirmationResendReq {
    [Required] public string Email { get; set; }
}
}