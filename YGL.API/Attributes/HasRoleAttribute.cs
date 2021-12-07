using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YGL.API.Attributes; 

public class HasRoleAttribute : ForbiddenActionFilterAttribute {
    private readonly string[] _roles;

    public HasRoleAttribute(string[] roles) {
        _roles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        try {
            var roleClaims = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            var roleCheck = roleClaims.Any(x => _roles.Any(y => y == x.Value));

            if (roleCheck) return;
        }
        catch (Exception) {
            ReturnForbidden(context);
            return;
        }

        ReturnForbidden(context);
    }
}