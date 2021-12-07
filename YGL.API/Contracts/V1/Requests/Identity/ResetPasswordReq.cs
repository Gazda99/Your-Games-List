using System.ComponentModel.DataAnnotations;

namespace YGL.API.Contracts.V1.Requests.Identity; 

public class ResetPasswordReq {
    [Required] public string ResetPasswordToken { get; set; }
    [Required] public string NewPassword { get; set; }
}