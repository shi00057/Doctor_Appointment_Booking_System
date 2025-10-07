using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Doctor.ViewModels;
using CST8002.Web.ViewModels;
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
            var dtos = await _notifications.ListForUserAsync(_user.UserId, unreadOnly: true, top: 20, ct);

            var unread = (dtos ?? Array.Empty<CST8002.Application.DTOs.NotificationDto>())
                .Select(d => new NotificationVm
                {
                    Id = d.NotificationId,
                    Message = !string.IsNullOrWhiteSpace(d.Title)
                                  ? (string.IsNullOrWhiteSpace(d.Body) ? d.Title : $"{d.Title} — {d.Body}")
                                  : (d.Body ?? string.Empty),
                    CreatedAt = DateTime.SpecifyKind(d.CreatedUtc, DateTimeKind.Utc).ToLocalTime(),
                    IsRead = d.IsRead,
                    Severity = d.Severity switch { 3 => "danger", 2 => "warning", 1 => "success", _ => "info" }
                })
                .ToArray();

            var vm = new DoctorDashboardIndexVm
            {
                DisplayName = User?.Identity?.Name ?? $"Doctor#{_user.UserId}",
                Today = DateTime.Today,
                UnreadCount = unread.Length,
                Unread = unread
            };

            ViewData["UnreadNotifications"] = vm.Unread; 
            ViewData["UnreadCount"] = vm.UnreadCount;

            return View(vm);
        }
    }
}
