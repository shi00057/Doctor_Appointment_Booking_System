using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)) return RedirectToAction("Admin");
            if (string.Equals(role, "Doctor", StringComparison.OrdinalIgnoreCase)) return RedirectToAction("Doctor");
            return RedirectToAction("Patient");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Admin()
        {
            var vm = new AdminDashboardVm
            {
                UserName = User.Identity?.Name ?? string.Empty,
                Today = DateTime.UtcNow.Date
            };
            return View(vm);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public IActionResult Doctor()
        {
            var vm = new DoctorDashboardVm
            {
                UserName = User.Identity?.Name ?? string.Empty,
                Today = DateTime.UtcNow.Date
            };
            return View(vm);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet]
        public IActionResult Patient()
        {
            var vm = new PatientDashboardVm
            {
                UserName = User.Identity?.Name ?? string.Empty,
                Today = DateTime.UtcNow.Date
            };
            return View(vm);
        }
    }
}
