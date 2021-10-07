using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Contracts.V1;
using YGL.API.Contracts.V1.Requests.Identity;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.Identity;
using YGL.API.Domain;
using YGL.API.Services.Controllers;

namespace YGL.API.Controllers.V1 {
public class IdentityController : ControllerBase {
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService) {
        _identityService = identityService;
    }

    [HttpPost(Routes.Identity.Login)]
    public async Task<IActionResult> Login([FromBody] UserLoginReq req) {
        AuthenticationResult authResult = await _identityService.LoginAsync(
            req.Username, req.Password);
        IResponse res;

        if (authResult.IsSuccess) {
            res = new LoginSuccessRes() {
                UserId = authResult.UserId,
                JwtToken = authResult.JwtToken,
                RefreshToken = authResult.RefreshToken
            }.ToResponse();
            return this.ReturnResult(authResult.StatusCode, res);
        }

        res = new LoginFailRes().WithErrors(authResult).ToResponseWithErrors();
        return this.ReturnResult(authResult.StatusCode, res);
    }


    [HttpPost(Routes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] UserRegisterReq req) {
        AuthenticationResult authResult =
            await _identityService.RegisterAsync(req.Email, req.Username, req.Password);
        IResponse res;

        if (authResult.IsSuccess) {
            res = new RegisterSuccessRes() {
                IsSuccess = true
            }.ToResponse();
            return this.ReturnResult(authResult.StatusCode, res, authResult.Location);
        }

        res = new RegisterFailRes().WithErrors(authResult).ToResponseWithErrors();
        return this.ReturnResult(authResult.StatusCode, res);
    }

    [HttpPost(Routes.Identity.Refresh)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenReq req) {
        AuthenticationResult authResult = await _identityService.RefreshTokenAsync(req.JwtToken, req.RefreshToken);
        IResponse res;

        if (authResult.IsSuccess) {
            res = new RefreshTokenSuccessRes() {
                JwtToken = authResult.JwtToken,
                RefreshToken = authResult.RefreshToken
            }.ToResponse();
            return this.ReturnResult(authResult.StatusCode, res);
        }

        res = new RefreshTokenFailRes().WithErrors(authResult).ToResponseWithErrors();
        return this.ReturnResult(authResult.StatusCode, res);
    }

    [HttpPost(Routes.Identity.EmailConfirmationResendEmail)]
    public async Task<IActionResult> EmailConfirmationResendEmail([FromBody] EmailConfirmationResendReq req) {
        EmailConfirmationResult emailConfirmationResult =
            await _identityService.ResendConfirmationEmailAsync(req.Email);
        IResponse res;

        if (emailConfirmationResult.IsSuccess) {
            res = new EmailConfirmationSuccessRes() { IsSuccess = true }.ToResponse();
            return this.ReturnResult(emailConfirmationResult.StatusCode, res);
        }

        res = new EmailConfirmationFailRes().WithErrors(emailConfirmationResult).ToResponseWithErrors();
        return this.ReturnResult(emailConfirmationResult.StatusCode, res);
    }

    [HttpGet(Routes.Identity.EmailConfirmation)]
    public async Task<IActionResult> EmailConfirmation([FromQuery] string url) {
        EmailConfirmationResult emailConfirmationResult = await _identityService.ConfirmEmailAsync(url);
        IResponse res;

        if (emailConfirmationResult.IsSuccess) {
            res = new EmailConfirmationSuccessRes() { IsSuccess = true }.ToResponse();
            return this.ReturnResult(emailConfirmationResult.StatusCode, res);
        }

        res = new EmailConfirmationFailRes().WithErrors(emailConfirmationResult).ToResponseWithErrors();
        return this.ReturnResult(emailConfirmationResult.StatusCode, res);
    }

    [HttpPost(Routes.Identity.ResetPasswordSendEmail)]
    public async Task<IActionResult> ResetPasswordSendEmail([FromBody] ResetPasswordSendEmailReq req) {
        PasswordResetResult passwordResetResult = await _identityService.SendResetPasswordEmailAsync(req.Email);
        IResponse res;

        if (passwordResetResult.IsSuccess) {
            res = new PasswordResetSendEmailSuccessRes() { IsSuccess = true }.ToResponse();
            return this.ReturnResult(passwordResetResult.StatusCode, res);
        }

        res = new PasswordResetSendEmailFailRes().WithErrors(passwordResetResult).ToResponseWithErrors();
        return this.ReturnResult(passwordResetResult.StatusCode, res);
    }


    [HttpGet(Routes.Identity.ResetPasswordConfirmation)]
    public async Task<IActionResult> ResetPasswordConfirmation([FromQuery] string url) {
        PasswordResetResult passwordResetResult = await _identityService.ConfirmResetPasswordAsync(url);
        IResponse res;

        if (passwordResetResult.IsSuccess) {
            res = new PasswordResetConfirmationSuccessRes()
                { PasswordResetToken = passwordResetResult.PasswordResetToken }.ToResponse();
            return this.ReturnResult(passwordResetResult.StatusCode, res);
        }

        res = new PasswordResetConfirmationFailRes().WithErrors(passwordResetResult).ToResponseWithErrors();
        return this.ReturnResult(passwordResetResult.StatusCode, res);
    }

    [HttpPost(Routes.Identity.ResetPassword)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReq req) {
        PasswordResetResult passwordResetResult =
            await _identityService.ResetPasswordAsync(req.ResetPasswordToken, req.NewPassword);
        IResponse res;

        if (passwordResetResult.IsSuccess) {
            res = new PasswordResetSuccessRes() { IsSuccess = true }.ToResponse();
            return this.ReturnResult(passwordResetResult.StatusCode, res);
        }

        res = new PasswordResetFailRes().WithErrors(passwordResetResult).ToResponseWithErrors();
        return this.ReturnResult(passwordResetResult.StatusCode, res);
    }
}
}