using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.Services;

namespace YGL.API.Installers; 

public class PasswordHasherInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddSingleton<ICustomPasswordHasher, CustomPasswordHasher>();
    }
}