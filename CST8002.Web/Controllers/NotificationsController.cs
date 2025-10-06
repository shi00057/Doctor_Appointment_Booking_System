using Azure.Core;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Repositories;
using CST8002.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Web.Controllers

{
    [Authorize]
    public sealed class NotificationsController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ICurrentUser _user;

        public NotificationsController(IUserRepository users, ICurrentUser user)
        {
            _users = users;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(long id, CancellationToken ct)
        {
            await _users.NotificationsMarkReadAsync(_user.UserId, id, ct);
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer)) return Redirect(referer);
            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAll(CancellationToken ct)
        {
            await _users.NotificationsMarkAllAsync(_user.UserId, ct);
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer)) return Redirect(referer);
            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }
    }
}
//{
//    [Authorize]
//    public sealed class NotificationsController : Controller
//    {
//        private readonly INotificationQueryService _notifications;
//        private readonly ICurrentUser _user;

//        public NotificationsController(INotificationQueryService notifications, ICurrentUser user)
//        {
//            _notifications = notifications;
//            _user = user;
//        }
//        private IActionResult SafeBack()
//        {
//            var referer = Request.Headers["Referer"].ToString();
//            if (!string.IsNullOrWhiteSpace(referer) && Url.IsLocalUrl(referer))
//                return Redirect(referer);

//            return RedirectToAction("Index", "Directory");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> MarkRead(long id, CancellationToken ct)
//        {
//            await _notifications.MarkReadAsync(_user.UserId, id, ct);
//            var referer = Request.Headers["Referer"].ToString();
//            if (string.IsNullOrWhiteSpace(referer)) referer = Url.Content("~/");
//            return Redirect(referer);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> MarkAll(CancellationToken ct)
//        {
//            await _notifications.MarkAllAsync(_user.UserId, ct);
//            var referer = Request.Headers["Referer"].ToString();
//            if (string.IsNullOrWhiteSpace(referer)) referer = Url.Content("~/");
//            return Redirect(referer);
//        }
//    }
//}


