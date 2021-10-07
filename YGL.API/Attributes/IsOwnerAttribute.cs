using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YGL.API.Attributes {
public class IsOwnerAttribute : ForbiddenActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        try {
            string pathValue = context.HttpContext.Request.Path.Value;

            if (String.IsNullOrEmpty(pathValue) || String.IsNullOrWhiteSpace(pathValue)) {
                ReturnForbidden(context);
                return;
            }

            long claimUserId = Int64.Parse(context.HttpContext.User.Claims.First(c => c.Type == "Id").Value);
            long requestItemId = Int64.Parse(pathValue.Split('/').Last().Trim());

            if (claimUserId == requestItemId) return;
        }
        catch (Exception) {
            ReturnForbidden(context);
            return;
        }

        ReturnForbidden(context);
    }
}
}