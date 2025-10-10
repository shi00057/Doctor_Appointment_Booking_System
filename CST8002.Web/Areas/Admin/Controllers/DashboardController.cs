using System;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Web.Areas.Admin.Controllers
{

    public sealed class DashboardController : AdminControllerBase 
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
            ViewData["UnreadNotifications"] = unread;
            ViewData["UnreadCount"] = unread?.Count ?? 0;
            var vm = new AdminDashboardIndexVm
            {
                UserName = User?.Identity?.Name ?? $"Admin#{_user.UserId}",               Today = DateTime.Today,
                PendingPatients = 0,   
                Doctors = 0,
                TodayAppointments = 0
            };
            return View(vm);
        }
    }
}
