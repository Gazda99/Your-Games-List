using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace YGL.API.Installers {
public class SwaggerInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        services.AddSwaggerGen(swaggerGenOptions => {
            swaggerGenOptions.SwaggerDoc("v1",
                new OpenApiInfo {
                    Title = "Your Games List API",
                    Version = "v1",
                    Description = "Service to support the Your Games List site"
                });
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                        { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                    Array.Empty<string>()
                }
            });
            //  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
    }
}
}