using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Identity;
using YGL.API.Contracts.V1.Requests.User;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers {
public interface IUserService {
    public Task<UserResult> GetUsers(string userIds);
    public Task<UserResult> GetUsersFilter(UserFilterQuery userFilterQuery, PaginationFilter paginationFilter = null);

    public Task<UserResult> UpdateUser(long userId, UpdateUserReq updateUserReq);

    public Task<UserResult> DeleteUser(long userId);
}
}