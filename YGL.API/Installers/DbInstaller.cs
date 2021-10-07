using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YGL.API.Installers {
public class DbInstaller : IInstaller {
    private const string YGLDbConnectionString = "YGLDBConnectionString";

    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<YGL.Model.YGLDataContext>(options => options.UseSqlServer(
            configuration.GetConnectionString(YGLDbConnectionString)));
    }
}
}