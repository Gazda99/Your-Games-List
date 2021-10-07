using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Identity;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface IUserService {
    public Task<UserResult> GetUser(long userId);
    public Task<UserResult> GetUsers(UserFilterQuery userFilterQuery, PaginationFilter paginationFilter = null);

    public Task<UserResult> UpdateUser(long userId, UpdateUserReq updateUserReq);

    public Task<UserResult> DeleteUser(long userId);
}
}