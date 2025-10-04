using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CST8002.Web.Filters
{
    public sealed class ExceptionHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var problem = new ProblemDetails
            {
                Title = "Server Error",
                Detail = context.Exception.Message,
                Status = StatusCodes.Status500InternalServerError
            };
            context.Result = new ObjectResult(problem) { StatusCode = StatusCodes.Status500InternalServerError };
            context.ExceptionHandled = true;
        }
    }
}
