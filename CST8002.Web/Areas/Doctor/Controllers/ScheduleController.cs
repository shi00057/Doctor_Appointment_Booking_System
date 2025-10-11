using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public sealed class ScheduleController : DoctorControllerBase
    {
        private readonly IScheduleService _schedule;
        private readonly IDoctorService _doctors;
        private readonly ICurrentUser _user;

        public ScheduleController(IScheduleService schedule, IDoctorService doctors, ICurrentUser user)
        {
            _schedule = schedule;
            _doctors = doctors;
            _user = user;
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
            if (vm.StartHour.HasValue && vm.EndHour.HasValue && vm.StartHour.Value >= vm.EndHour.Value)
            {
                ModelState.AddModelError(string.Empty, "StartHour must be < EndHour.");
            }

            if (!ModelState.IsValid) return View(vm);

            var doctorId = await ResolveDoctorIdAsync(ct);

            var req = new GenerateSlotsRequest
            {
                DoctorId = doctorId,
                WorkDate = vm.WorkDate!.Value.Date,
                StartHour = vm.StartHour!.Value,
                EndHour = vm.EndHour!.Value
            };

            await _schedule.DoctorGenerateSlotsAsync(req, ct);
            TempData["Ok"] = "Generated";
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> ResolveDoctorIdAsync(CancellationToken ct)
        {
            var doctorId = await _doctors.GetDoctorIdByUserIdAsync(ct);
            if (doctorId <= 0)
                throw new InvalidOperationException($"Doctor not found for current user (UserId={_user.UserId}).");
            return doctorId;
        }
    }
}
