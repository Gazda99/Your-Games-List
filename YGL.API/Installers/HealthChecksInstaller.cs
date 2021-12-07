using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.HealthChecks;
using YGL.Model;

namespace YGL.API.Installers; 

public class HealthChecksInstaller : IInstaller {
    private const string YGLDBHealtCheckName = "YGLDB";
    private const string RedisHealthCheckName = "Redis";

    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddHealthChecks()
            .AddDbContextCheck<YGLDataContext>(YGLDBHealtCheckName)
            .AddCheck<RedisHealthCheck>(RedisHealthCheckName);
    }
}