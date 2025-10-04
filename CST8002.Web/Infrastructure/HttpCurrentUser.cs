using System.Security.Claims;
using CST8002.Application.Abstractions;

namespace CST8002.Web.Infrastructure
{
    public sealed class HttpCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        public HttpCurrentUser(IHttpContextAccessor http) => _http = http;
        public int UserId
        {
            get
            {
                var id = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(id, out var v) ? v : 0;
            }
        }
        public string Role => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}
