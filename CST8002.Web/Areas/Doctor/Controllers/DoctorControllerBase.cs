using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    [AutoValidateAntiforgeryToken]
    public abstract class DoctorControllerBase : Controller
    {
        protected object AreaRouteValues => new { area = "Doctor" };

        protected IActionResult RedirectToDashboard() =>
            RedirectToAction("Index", "Dashboard", AreaRouteValues);

        protected IActionResult RedirectToAppointments() =>
            RedirectToAction("Index", "Appointments", AreaRouteValues);

        protected IActionResult RedirectToSchedule() =>
            RedirectToAction("Index", "Schedule", AreaRouteValues);

        protected void SetUnreadCount(int count) =>
            ViewData["UnreadCount"] = count;

        protected void Toast(string message, string type = "info")
        {
            TempData["Toast.Message"] = message;
            TempData["Toast.Type"] = type;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Area"] = "Doctor";
            base.OnActionExecuting(context);
        }
    }
}
