using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.Identity;
using YGL.API.Contracts.V1.Requests.User;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.User;
using YGL.API.Domain;
using YGL.API.EnumTypes;
using YGL.API.Services.IControllers;


namespace YGL.API.Controllers.V1 {
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase {
    private readonly IUserService _userService;


    public UserController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet(Routes.User.GetUser)]
    public async Task<IActionResult> GetUsers(string userIds) {
        UserResult userResult = await _userService.GetUsers(userIds);
        IResponse res;

        if (userResult.IsSuccess) {
            res = userResult.Users
                .Select(u => new UserGetSuccRes() { User = u })
                .ToResponse(userResult.Users.Count);

            return this.ReturnResult(userResult.StatusCode, res);
        }

        res = new UserDeleteFailRes() {
            ErrorCodes = userResult.ErrorCodes,
            ErrorMessages = userResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(userResult.StatusCode, res);
    }


    [HttpGet(Routes.User.GetUsers)]
    public async Task<IActionResult> GetUsersFilter(
        [FromQuery] UserFilterQuery userFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        UserResult userResult = await _userService.GetUsersFilter(userFilterQuery, (PaginationFilter)paginationQuery);
        IResponse res;

        if (userResult.IsSuccess) {
            res = userResult.Users
                .Select(u => new UserGetSuccRes() { User = u })
                .ToPagedResponse(userResult.Users.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(userResult.StatusCode, res);
        }

        res = new UserGetFailRes() {
            ErrorCodes = userResult.ErrorCodes,
            ErrorMessages = userResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(userResult.StatusCode, res);
    }


    [HttpPut(Routes.User.UpdateUser)]
    [IsOwnerOrHasRoleAttribute(new[] { Role.Admin })]
    public async Task<IActionResult> UpdateUser([FromRoute] long userId, [FromBody] UpdateUserReq updateUserReq) {
        UserResult userResult = await _userService.UpdateUser(userId, updateUserReq);
        IResponse res;

        if (userResult.IsSuccess) {
            res = new UserUpdateSuccRes() { IsSuccess = true }.ToSingleResponse();
            return this.ReturnResult(userResult.StatusCode, res);
        }

        res = new UserUpdateFailRes() {
            ErrorCodes = userResult.ErrorCodes,
            ErrorMessages = userResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(userResult.StatusCode, res);
    }


    [HttpDelete(Routes.User.DeleteUser)]
    [IsOwnerOrHasRoleAttribute(new[] { Role.Admin })]
    public async Task<IActionResult> DeleteUser([FromRoute] long userId) {
        UserResult userResult = await _userService.DeleteUser(userId);
        IResponse res;

        if (userResult.IsSuccess) {
            res = new UserDeleteSuccRes() { IsSuccess = true }.ToSingleResponse();
            return this.ReturnResult(userResult.StatusCode, res);
        }

        res = new UserDeleteFailRes() {
            ErrorCodes = userResult.ErrorCodes,
            ErrorMessages = userResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(userResult.StatusCode, res);
    }
}
}