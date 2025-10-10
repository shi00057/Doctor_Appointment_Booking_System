using System.Linq;
using Microsoft.AspNetCore.Http;      
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;     

namespace CST8002.Web.Filters
{
    public sealed class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var endpoint = context.HttpContext.GetEndpoint();
            var isApi = endpoint?.Metadata?.GetMetadata<ApiControllerAttribute>() is not null;

            var req = context.HttpContext.Request;
            var typed = req.GetTypedHeaders();

            var wantsJson = (typed.Accept?.Any(mt => IsJsonLike(mt)) ?? false);

            var isJsonBody = req.HasJsonContentType();

            if (isApi || wantsJson || isJsonBody)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private static bool IsJsonLike(MediaTypeHeaderValue mt)
        {
            var seg = mt.MediaType;          
            if (!seg.HasValue) return false; 
            var media = seg.Value;          
            if (string.IsNullOrEmpty(media)) return false;

            return media.Equals("application/json", System.StringComparison.OrdinalIgnoreCase)
                || media.Equals("text/json", System.StringComparison.OrdinalIgnoreCase)
                || media.EndsWith("+json", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
