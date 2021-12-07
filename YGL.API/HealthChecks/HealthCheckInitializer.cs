using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using YGL.API.Contracts;

namespace YGL.API.HealthChecks;

public static class HealthCheckInitializer {
    private const string HealthCheckEndpointUrl = "/health";
    private  const string NoneDescription = "none";

    public static void SetHealthChecks(IApplicationBuilder app) {
        app.UseHealthChecks(HealthCheckEndpointUrl, new HealthCheckOptions() { ResponseWriter = HealthCheckResponseWriter });
    }

    private static async Task HealthCheckResponseWriter(HttpContext context, HealthReport report) {
   

        var jsonSerializerSettings = new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        context.Response.ContentType = ContentTypes.ApplicationJson;

        var response = new HealthCheckRes() {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(e =>
                new HealthCheck() {
                    Component = e.Key,
                    Status = e.Value.Status.ToString(),
                    Description = e.Value.Description ?? NoneDescription
                }),
            Duration = report.TotalDuration,
            Date = DateTime.UtcNow
        };

        var stringResponse = JsonConvert.SerializeObject(response, jsonSerializerSettings);
        await context.Response.WriteAsync(stringResponse);
    }
}