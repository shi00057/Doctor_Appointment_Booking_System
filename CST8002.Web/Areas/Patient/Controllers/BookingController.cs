using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Patient.ViewModels;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public sealed class BookingController : PatientControllerBase
    {
        private readonly IDoctorService _doctors;
        private readonly IScheduleService _schedule;
        private readonly IAppointmentService _appointments;
        private readonly IUserContextService _userCtx;
        private readonly ICurrentUser _user;

        public BookingController(
            IDoctorService doctors,
            IScheduleService schedule,
            IAppointmentService appointments,
            IUserContextService userCtx,
            ICurrentUser user)
        {
            _doctors = doctors;
            _schedule = schedule;
            _appointments = appointments;
            _userCtx = userCtx;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var doctors = await _doctors.ListBasicsAsync(ct);
            var vm = new PatientBookingIndexVm
            {
                WorkDate = DateTime.UtcNow.Date,
                Doctors = doctors
                    .Select(d => new SelectListItem { Value = d.DoctorId.ToString(), Text = string.IsNullOrWhiteSpace(d.Specialty) ? d.Name : $"{d.Name} - {d.Specialty}" })
                    .ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Slots(int doctorId, DateTime date, CancellationToken ct)
        {
            var slots = await _schedule.ListAvailableSlotsAsync(doctorId, date, ct);
            var result = slots.Select(s => new SlotVm { StartUtc = s.StartUtc, EndUtc = s.EndUtc });
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PatientBookingIndexVm vm, CancellationToken ct)
        {
            var pid = await _userCtx.GetPatientIdAsync(_user.UserId, ct);
            if (pid is null) return Forbid();

            var req = new CreateAppointmentRequest
            {
                DoctorId = vm.DoctorId,
                PatientId = pid.Value,
                StartUtc = vm.StartUtc,
                EndUtc = vm.EndUtc
            };

            await _appointments.BookAsync(req, ct);
            return RedirectToAction("Index", "Appointments", new { area = "Patient" });
        }
    }
}
