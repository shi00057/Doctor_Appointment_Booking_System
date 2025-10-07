using System;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Patient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public sealed class DashboardController : PatientControllerBase
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
            var dtos = await _notifications.ListForUserAsync(_user.UserId, true, 20, ct);

            var unread = (dtos ?? Array.Empty<CST8002.Application.DTOs.NotificationDto>())
                .Select(d => new CST8002.Web.ViewModels.NotificationVm
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

            ViewData["UnreadNotifications"] = unread;
            ViewData["UnreadCount"] = unread.Length;

            var vm = new PatientDashboardIndexVm
            {
                DisplayName = User?.Identity?.Name ?? "Patient",
                Today = DateTime.Now,
                UnreadCount = unread.Length
            };

            return View(vm);
        }

    }
}
