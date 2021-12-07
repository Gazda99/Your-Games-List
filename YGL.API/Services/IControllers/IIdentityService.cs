using System.Threading.Tasks;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers; 

public interface IIdentityService {
    public Task<AuthenticationResult> LoginAsync(string username, string password);

    public Task<AuthenticationResult> RegisterAsync(
        string email,
        string username,
        string password
    );

    public Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);

    public Task<EmailConfirmationResult> ConfirmEmailAsync(string url);

    public Task<EmailConfirmationResult> ResendConfirmationEmailAsync(string email);

    public Task<PasswordResetResult> SendResetPasswordEmailAsync(string email);
    public Task<PasswordResetResult> ConfirmResetPasswordAsync(string url);
    public Task<PasswordResetResult> ResetPasswordAsync(string passwordResetToken,string newPassword);
}