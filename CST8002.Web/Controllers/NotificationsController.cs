using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ui/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IUserRepository _users;

        public NotificationsController(IUserRepository users)
        {
            _users = users;
        }

        [HttpPost("mark-read/{id:long}")]
        public async Task<IActionResult> MarkRead(long id, CancellationToken ct)
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid) || uid <= 0) return Unauthorized();
            await _users.NotificationsMarkReadAsync(uid, id, ct);
            return Ok();
        }
    }
}
