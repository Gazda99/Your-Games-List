using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YGL.API.Attributes {
public class IsOwnerOrHasRoleAttribute : ForbiddenActionFilterAttribute {
    private readonly string[] _roles;

    public IsOwnerOrHasRoleAttribute(string[] roles) {
        _roles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        try {
            string pathValue = context.HttpContext.Request.Path.Value;

            if (String.IsNullOrEmpty(pathValue) || String.IsNullOrWhiteSpace(pathValue)) 
            {
                ReturnForbidden(context);
                return;
            }

            long claimUserId;
            long requestItemId;
            try {
                claimUserId = Int64.Parse(context.HttpContext.User.Claims.First(c => c.Type == "Id").Value);
                requestItemId = Int64.Parse(pathValue.Split('/').Last().Trim());
            }
            catch (Exception) {
                ReturnForbidden(context);
                return;
            }
            
            if (claimUserId == requestItemId)
                return;

            var roleClaims = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            bool roleCheck = roleClaims.Any(x => _roles.Any(y => y == x.Value));

            if (roleCheck) return;
        }
        catch (Exception) {
            ReturnForbidden(context);
            return;
        }

        ReturnForbidden(context);
    }
}
}