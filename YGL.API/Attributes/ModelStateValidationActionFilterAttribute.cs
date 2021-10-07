using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace YGL.API.Attributes {
public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        ModelStateDictionary modelState = context.ModelState;
        if (!modelState.IsValid)
            context.Result = new BadRequestObjectResult(context.ModelState);
    }
}
}