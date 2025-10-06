using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public sealed class DashboardController : DoctorControllerBase
    {
        private readonly INotificationQueryService _notifications;
        private readonly ICurrentUser _user;

        public DashboardController(INotificationQueryService notifications, ICurrentUser user)
        {
            _notifications = notifications;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var unread = await _notifications.ListForUserAsync(_user.UserId, true, 20, ct);
            ViewData["UnreadCount"] = unread?.Count ?? 0;
            return View();
        }
    }
}
