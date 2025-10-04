using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;

namespace CST8002.Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorUiController : Controller
    {
        private readonly IScheduleService _schedule;
        private readonly IAppointmentService _appointments;
        private readonly IDoctorService _doctors;
        private readonly IDateTimeProvider _clock;

        public DoctorUiController(
            IScheduleService schedule,
            IAppointmentService appointments,
            IDoctorService doctors,
            IDateTimeProvider clock)
        {
            _schedule = schedule;
            _appointments = appointments;
            _doctors = doctors;
            _clock = clock;
        }

        [HttpGet]
        public IActionResult Index(int doctorId)
        {
            ViewBag.DoctorId = doctorId;
            ViewBag.Today = _clock.UtcNow.ToLocalTime().Date;
            return View();
        }

        [HttpGet]
        public IActionResult Schedule(int doctorId)
        {
            var vm = new Models.GenerateSlotsVm
            {
                DoctorId = doctorId,
                WorkDate = _clock.UtcNow.ToLocalTime().Date,
                StartHour = 9,
                EndHour = 17
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(Models.GenerateSlotsVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var req = new GenerateSlotsRequest
            {
                DoctorId = vm.DoctorId,
                WorkDate = vm.WorkDate,
                StartHour = vm.StartHour,
                EndHour = vm.EndHour
            };
            await _schedule.DoctorGenerateSlotsAsync(req, ct);
            vm.Message = "Slots generated.";
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointments(int doctorId, DateTime? from, DateTime? to, CancellationToken ct)
        {
            var now = _clock.UtcNow;
            var fromUtc = from?.ToUniversalTime() ?? now.AddDays(-30);
            var toUtc = to?.ToUniversalTime() ?? now.AddDays(30);
            var list = await _appointments.ListByDoctorAsync(doctorId, fromUtc, toUtc, ct);
            var vm = new Models.MyAppointmentsVm
            {
                DoctorId = doctorId,
                From = fromUtc.ToLocalTime(),
                To = toUtc.ToLocalTime(),
                Items = list.OrderByDescending(x => x.StartUtc).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult GenerateSchedule(int doctorId) => RedirectToAction(nameof(Schedule), new { doctorId });

        [HttpGet]
        public IActionResult ManageAppointments(int doctorId) => RedirectToAction(nameof(MyAppointments), new { doctorId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(long apptId, int doctorId, string reason, CancellationToken ct)
        {
            reason ??= "Cancelled by doctor";
            await _appointments.AdminCancelAsync(apptId, reason, ct);
            return RedirectToAction(nameof(MyAppointments), new { doctorId });
        }

        [HttpGet]
        public IActionResult Profile(int doctorId)
        {
            var vm = new Models.DoctorProfileVm
            {
                DoctorId = doctorId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Models.DoctorProfileVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new DoctorDto
            {
                DoctorId = vm.DoctorId,
                Name = vm.Name,
                Specialty = vm.Specialty,
                Email = vm.Email,
                Phone = vm.Phone,
                IsActive = vm.IsActive
            };
            var saved = await _doctors.UpdateAsync(dto, ct);
            vm.Name = saved.Name;
            vm.Specialty = saved.Specialty;
            vm.Email = saved.Email;
            vm.Phone = saved.Phone;
            vm.IsActive = saved.IsActive;
            vm.Message = "Profile saved.";
            return View(vm);
        }
    }
}
