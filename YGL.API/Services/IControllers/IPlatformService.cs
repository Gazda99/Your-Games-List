using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Platform;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers {
public interface IPlatformService {
    public Task<PlatformResult> GetPlatforms(string platformIds);
    public Task<PlatformResult> GetPlatformsFilter(PlatformFilterQuery platformFilterQuery, PaginationFilter paginationFilter);
}
}