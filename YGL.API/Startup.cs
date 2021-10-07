using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YGL.API.Installers;

namespace YGL.API {
public class Startup {
    private const string ApiSwaggerName = "YGLApi v1";
    private const string CorsPolicy = "AllowAll";
    private const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";

    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
        ServicesInstaller servicesInstaller = new ServicesInstaller();
        servicesInstaller.InstallServicesInOrder(services, Configuration);
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(SwaggerEndpointUrl, ApiSwaggerName));
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors(CorsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
}