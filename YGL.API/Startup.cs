using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using YGL.API.Contracts;
using YGL.API.Controllers.V1;
using YGL.API.HealthChecks;
using YGL.API.Installers;

namespace YGL.API {
public class Startup {
    private const string ApiSwaggerName = "YGLApi v1";
    private const string CorsPolicy = "AllowAll";
    private const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";
    private const string HealthCheckEndpointUrl = "/health";

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

        SetHealthChecks(app, env);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors(CorsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void SetHealthChecks(IApplicationBuilder app, IWebHostEnvironment env) {
        app.UseHealthChecks(HealthCheckEndpointUrl, new HealthCheckOptions() {
            ResponseWriter = (HttpContext context, HealthReport report) => HealthCheckResponseWriter(context, report)
        });
    }

    private async Task HealthCheckResponseWriter(HttpContext context, HealthReport report) {
        const string noneDescription = "none";
        
        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        context.Response.ContentType = ContentTypes.ApplicationJson;

        HealthCheckRes response = new HealthCheckRes() {
            Status = report.Status.ToString(),
            Checks = report.Entries
                .Select(e => new HealthCheck() {
                    Component = e.Key,
                    Status = e.Value.Status.ToString(),
                    Description = e.Value.Description ?? noneDescription
                }),
            Duration = report.TotalDuration,
            Date = DateTime.UtcNow
        };

        string stringResponse = JsonConvert.SerializeObject(response, jsonSerializerSettings);
        await context.Response.WriteAsync(stringResponse);
    }
}
}