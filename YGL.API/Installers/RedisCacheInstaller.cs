using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.Services;
using YGL.API.Settings;

namespace YGL.API.Installers {
public class RedisCacheInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        RedisCacheSettings redisCacheSettings = new RedisCacheSettings();
        configuration.Bind(nameof(redisCacheSettings), redisCacheSettings);
        services.AddSingleton(redisCacheSettings);

        if (!redisCacheSettings.Enabled)
            return;

        services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
    }
}
}