using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.Identity;
using YGL.API.Domain;
using YGL.API.Domain.SafeObjects;
using YGL.API.Errors;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers {
public class UserService : IUserService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public UserService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }


    public async Task<UserResult> GetUser(long userId) {
        UserResult userResult = new UserResult();
        YGL.Model.User foundUser =
            await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.ItemStatus == true);


        if (foundUser is null) {
            userResult.IsSuccess = false;
            userResult.StatusCode = HttpStatusCode.NotFound;
            userResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UserNotFound);
            return userResult;
        }

        userResult.Users = new List<SafeUser>() { new SafeUser(foundUser) };
        userResult.IsSuccess = true;
        userResult.StatusCode = HttpStatusCode.OK;
        return userResult;
    }

    public async Task<UserResult> GetUsers(UserFilterQuery userFilterQuery, PaginationFilter paginationFilter) {
        UserResult userResult = new UserResult();

        IQueryable<YGL.Model.User> usersQueryable = _yglDataContext.Users
            .Where(u => u.ItemStatus == true);

        usersQueryable = AddFiltersOnQueryGetUsers(userFilterQuery, usersQueryable);

        List<SafeUser> safeUsers =
            (await usersQueryable
                .Include(u => u.Groups)
                .Include(u => u.ListOfGames)
                .ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(u => new SafeUser(u));

        userResult.Users = safeUsers;

        userResult.IsSuccess = true;
        userResult.StatusCode = HttpStatusCode.OK;
        return userResult;
    }

    public async Task<UserResult> UpdateUser(long userId, UpdateUserReq updateUserReq) {
        UserResult userResult = new UserResult();
        YGL.Model.User foundUser =
            await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.ItemStatus == true);

        if (foundUser is null) {
            userResult.IsSuccess = false;
            userResult.StatusCode = HttpStatusCode.NotFound;
            userResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UserNotFound);
            return userResult;
        }


        if (!ValidationUser.ValidateUserAbout(updateUserReq.About, userResult))
            return UserResultUnprocessableEntity(userResult);

        if (!ValidationUser.ValidateCountry(updateUserReq.Country, userResult))
            return UserResultUnprocessableEntity(userResult);

        if (!ValidationUser.ValidateGender(updateUserReq.Gender, userResult))
            return UserResultUnprocessableEntity(userResult);

        if (!ValidationUser.ValidateBirthYear(updateUserReq.BirthYear, userResult))
            return UserResultUnprocessableEntity(userResult);

        if (!ValidationUser.ValidateSlug(updateUserReq.Slug, userResult))
            return UserResultUnprocessableEntity(userResult);

        foundUser.About = updateUserReq.About;
        foundUser.Country = updateUserReq.Country;
        foundUser.Gender = updateUserReq.Gender;
        foundUser.BirthYear = updateUserReq.BirthYear;
        foundUser.Slug = updateUserReq.Slug;


        _yglDataContext.Users.Update(foundUser);
        await _yglDataContext.SaveChangesAsync();

        userResult.IsSuccess = true;
        userResult.StatusCode = HttpStatusCode.OK;
        return userResult;
    }

    private static UserResult UserResultUnprocessableEntity(UserResult userResult) {
        userResult.IsSuccess = false;
        userResult.StatusCode = HttpStatusCode.UnprocessableEntity;
        return userResult;
    }


    public async Task<UserResult> DeleteUser(long userId) {
        UserResult userResult = new UserResult();
        YGL.Model.User foundUser =
            await _yglDataContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.ItemStatus == true);

        if (foundUser is null) {
            userResult.IsSuccess = false;
            userResult.StatusCode = HttpStatusCode.NotFound;
            userResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.UserNotFound);
            return userResult;
        }

        foundUser.ItemStatus = false;
        _yglDataContext.Users.Update(foundUser);
        await _yglDataContext.SaveChangesAsync();

        userResult.IsSuccess = true;
        userResult.StatusCode = HttpStatusCode.OK;
        return userResult;
    }

    private static IQueryable<YGL.Model.User> AddFiltersOnQueryGetUsers(UserFilterQuery filterQuery,
        IQueryable<YGL.Model.User> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.Username))
            queryable = queryable.Where(u => u.Username.Contains(filterQuery.Username));

        return queryable;
    }
}
}