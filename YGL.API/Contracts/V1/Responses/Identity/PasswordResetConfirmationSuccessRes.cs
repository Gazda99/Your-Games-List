namespace YGL.API.Contracts.V1.Responses.Identity; 

public class PasswordResetConfirmationSuccessRes : IObjectForResponse {
    public string PasswordResetToken { get; set; }
}