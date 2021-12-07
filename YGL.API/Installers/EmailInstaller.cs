using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.Services;
using YGL.API.Settings;

namespace YGL.API.Installers; 

public class EmailInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        var confirmationEmailSettings = new ConfirmationEmailSettings();
        configuration.Bind(EmailConfigurationKey(nameof(confirmationEmailSettings)), confirmationEmailSettings);
        services.AddSingleton(confirmationEmailSettings);

        var passwordResetEmailSettings = new PasswordResetEmailSettings();
        configuration.Bind(EmailConfigurationKey(nameof(passwordResetEmailSettings)), passwordResetEmailSettings);
        services.AddSingleton(passwordResetEmailSettings);

        services.AddScoped<EmailSender<AccountConfirmationEmailSender>>(_ =>
            new AccountConfirmationEmailSender(confirmationEmailSettings));

        services.AddScoped<EmailSender<PasswordResetEmailSender>>(_ =>
            new PasswordResetEmailSender(passwordResetEmailSettings));
    }

    private static string EmailConfigurationKey(string nameOfSetting) {
        return $"EmailSettings:{nameOfSetting}";
    }
}