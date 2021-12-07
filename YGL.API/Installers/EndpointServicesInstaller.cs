using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGL.API.Services.Controllers;
using YGL.API.Services.IControllers;

namespace YGL.API.Installers; 

public class EndpointServicesInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IGameModeService, GameModeService>();
        services.AddScoped<IPlatformService, PlatformService>();
        services.AddScoped<IPlayerPerspectiveService, PlayerPerspectiveService>();
    }
}