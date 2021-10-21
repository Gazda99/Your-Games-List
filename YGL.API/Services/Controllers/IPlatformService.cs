using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Platform;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface IPlatformService {
    public Task<PlatformResult> GetPlatform(int platformId);
    public Task<PlatformResult> GetPlatforms(PlatformFilterQuery platformFilterQuery, PaginationFilter paginationFilter);
}
}