using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public sealed class NotificationsController : PatientControllerBase
    {
        private readonly INotificationQueryService _notifications;
        private readonly ICurrentUser _user;

        public NotificationsController(INotificationQueryService notifications, ICurrentUser user)
        {
            _notifications = notifications;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool unreadOnly = false, int top = 50, CancellationToken ct = default)
        {
            if (top < 1) top = 1;
            if (top > 500) top = 500;

            var dtos = await _notifications.ListForUserAsync(_user.UserId, unreadOnly, top, ct);

            var vms = (dtos ?? Array.Empty<CST8002.Application.DTOs.NotificationDto>())
                .Select(d => new NotificationVm
                {
                    Id = d.NotificationId,
                    Message = !string.IsNullOrWhiteSpace(d.Title)
                        ? (string.IsNullOrWhiteSpace(d.Body) ? d.Title : $"{d.Title} — {d.Body}")
                        : (d.Body ?? string.Empty),
                    CreatedAt = DateTime.SpecifyKind(d.CreatedUtc, DateTimeKind.Utc).ToLocalTime(),
                    IsRead = d.IsRead,
                    Severity = d.Severity switch { 3 => "danger", 2 => "warning", 1 => "success", _ => "info" },
                    // LinkText/LinkUrl 可按需要从 RelatedEntity* 推导
                })
                .ToList();

            ViewData["UnreadCount"] = vms.Count(x => !x.IsRead);
            return View(vms);
        }

        private IActionResult SafeBack()
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer) && Url.IsLocalUrl(referer))
                return Redirect(referer);

            return RedirectToAction("Index", "Dashboard", new { area = "Patient" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(long id, CancellationToken ct = default)
        {
            await _notifications.MarkReadAsync(_user.UserId, id, ct);
            return SafeBack();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAll(CancellationToken ct = default)
        {
            await _notifications.MarkAllAsync(_user.UserId, ct);
            return SafeBack();
        }
    }
}
