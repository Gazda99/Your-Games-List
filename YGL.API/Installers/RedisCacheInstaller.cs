using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using YGL.API.Services;
using YGL.API.Settings;

namespace YGL.API.Installers; 

public class RedisCacheInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        var redisCacheSettings = new RedisCacheSettings();
        configuration.Bind(nameof(redisCacheSettings), redisCacheSettings);
        services.AddSingleton(redisCacheSettings);

        if (!redisCacheSettings.Enabled)
            return;

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));
        
        services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
    }
}