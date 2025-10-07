// Areas/Doctor/Controllers/DashboardController.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
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
        private readonly IMapper _mapper;

        public DashboardController(INotificationQueryService notifications, ICurrentUser user, IMapper mapper)
        {
            _notifications = notifications;
            _user = user;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var dtos = await _notifications.ListForUserAsync(_user.UserId, unreadOnly: true, top: 20, ct);
            var unread = _mapper.Map<NotificationVm[]>(dtos ?? Array.Empty<NotificationDto>());

            var vm = new DoctorDashboardIndexVm
            {
                DisplayName = User?.Identity?.Name ?? "Doctor",
                Today = DateTime.Today,
                UnreadCount = unread.Length,
                Unread = unread
            };

            ViewData["UnreadCount"] = vm.UnreadCount;
            return View(vm);
        }
    }
}
