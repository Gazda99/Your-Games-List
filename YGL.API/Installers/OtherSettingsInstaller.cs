using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using YGL.API.Settings;

namespace YGL.API.Installers; 

public class OtherSettingsInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        var passwordResetSettings = new PasswordResetSettings();
        configuration.Bind(nameof(passwordResetSettings), passwordResetSettings);
        services.AddSingleton(passwordResetSettings);
    }
}