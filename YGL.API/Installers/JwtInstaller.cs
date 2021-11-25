using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.Identity;
using YGL.API.Errors;
using YGL.API.Services;
using YGL.API.Settings;

namespace YGL.API.Installers {
public class JwtInstaller : IInstaller {
    public void InstallServices(IServiceCollection services, IConfiguration configuration) {
        JwtSettings jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);

        TokenValidationParametersWithLifetime tokenValidationParametersWithLifetime =
            new TokenValidationParametersWithLifetime(jwtSettings);

        TokenValidationParametersWithoutLifetime tokenValidationParametersWithoutLifetime =
            new TokenValidationParametersWithoutLifetime(jwtSettings);


        services.AddSingleton(tokenValidationParametersWithLifetime);
        services.AddSingleton(tokenValidationParametersWithoutLifetime);

        services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParametersWithLifetime;

                x.Events = new JwtBearerEvents() {
                    OnChallenge = JwtBearerCustomOnChallenge,
                    OnForbidden = JwtBearerCustomOnForbidden,
                };
            });
    }

    private Task JwtBearerCustomOnForbidden(ForbiddenContext context) {
        TokenValidationFailRes tokenValidationFailRes = new TokenValidationFailRes();

        tokenValidationFailRes.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.Forbidden);

        context.Response.ContentType = ContentTypes.ApplicationJson;
        context.Response.StatusCode = 403;
        
        tokenValidationFailRes.ToSingleResponse();

        return context.Response.WriteAsync(JsonConvert.SerializeObject(tokenValidationFailRes));
    }

    private Task JwtBearerCustomOnChallenge(JwtBearerChallengeContext context) {
        // Skip the default logic.
        context.HandleResponse();

        TokenValidationFailRes tokenValidationFailRes = new TokenValidationFailRes();

        if (context.ErrorDescription is not null && context.ErrorDescription.Contains("The token expired"))
            tokenValidationFailRes.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.JwtTokenExpired);

        else
            tokenValidationFailRes.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.JwtTokenValidationError);


        context.Response.ContentType = ContentTypes.ApplicationJson;
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        
        tokenValidationFailRes.ToSingleResponse();

        return context.Response.WriteAsync(JsonConvert.SerializeObject(tokenValidationFailRes));
    }
}
}