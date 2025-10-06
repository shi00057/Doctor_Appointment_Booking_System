using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public sealed class ScheduleController : DoctorControllerBase
    {
        private readonly IScheduleService _schedule;
        private readonly IDoctorService _doctors;

        public ScheduleController(IScheduleService schedule, IDoctorService doctors)
        {
            _schedule = schedule;
            _doctors = doctors;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new DoctorScheduleIndexVm();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DoctorScheduleIndexVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var doctorId = await ResolveDoctorIdAsync(ct);
            var req = new GenerateSlotsRequest
            {
                DoctorId = doctorId,
                WorkDate = vm.WorkDate.Date,
                StartHour = vm.StartHour,
                EndHour = vm.EndHour
            };

            await _schedule.DoctorGenerateSlotsAsync(req, ct);
            TempData["Ok"] = "Generated";
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> ResolveDoctorIdAsync(CancellationToken ct)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? string.Empty;
            var list = await _doctors.ListBasicsAsync(ct);
            var me = list.FirstOrDefault(x => string.Equals(x.Email ?? string.Empty, email, StringComparison.OrdinalIgnoreCase));
            if (me is null) throw new InvalidOperationException("Doctor not found for current user.");
            return me.DoctorId;
        }
    }
}
