using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CST8002.Web.Filters
{
    public sealed class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState);
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
