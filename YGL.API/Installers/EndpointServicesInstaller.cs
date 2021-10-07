using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.Services.Controllers;

namespace YGL.API.Installers {
public class EndpointServicesInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyService, CompanyService>();
    }
}
}