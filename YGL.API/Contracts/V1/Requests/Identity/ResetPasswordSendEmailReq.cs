using System.ComponentModel.DataAnnotations;

namespace YGL.API.Contracts.V1.Requests.Identity; 

public class ResetPasswordSendEmailReq {
    [Required] [EmailAddress] public string Email { get; set; }
}