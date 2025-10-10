using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Patient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<BookingController> _logger;
        private readonly IWebHostEnvironment _env;

        public BookingController(
            IDoctorService doctors,
            IScheduleService schedule,
            IAppointmentService appointments,
            IUserContextService userCtx,
            ICurrentUser user,
            ILogger<BookingController> logger,
            IWebHostEnvironment env)
        {
            _doctors = doctors;
            _schedule = schedule;
            _appointments = appointments;
            _userCtx = userCtx;
            _user = user;
            _logger = logger;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var doctors = await _doctors.ListBasicsAsync(ct) ?? Enumerable.Empty<DoctorDto>();

            var items = doctors
                .Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = string.IsNullOrWhiteSpace(d.Specialty) ? d.Name : $"{d.Name} - {d.Specialty}"
                })
                .Prepend(new SelectListItem { Value = "", Text = "-- Select a doctor --" })
                .ToList();

            var vm = new PatientBookingIndexVm
            {
                WorkDate = DateTime.UtcNow.Date,
                Doctors = items
            };
            return View(vm);
        }

        // This POST action matches your form (asp-action="Index" method="post")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PatientBookingIndexVm vm, CancellationToken ct)
        {
            if (vm.DoctorId <= 0)
            {
                ModelState.AddModelError(nameof(vm.DoctorId), "Please select a doctor.");
            }
            if (vm.StartUtc == default || vm.EndUtc == default)
            {
                ModelState.AddModelError(string.Empty, "Please pick a slot.");
            }
            if (vm.EndUtc <= vm.StartUtc)
            {
                ModelState.AddModelError(string.Empty, "Invalid time range.");
            }

            if (!ModelState.IsValid)
            {
                var doctors = await _doctors.ListBasicsAsync(ct) ?? Enumerable.Empty<DoctorDto>();
                vm.Doctors = doctors
                    .Select(d => new SelectListItem
                    {
                        Value = d.DoctorId.ToString(),
                        Text = string.IsNullOrWhiteSpace(d.Specialty) ? d.Name : $"{d.Name} - {d.Specialty}"
                    })
                    .Prepend(new SelectListItem { Value = "", Text = "-- Select a doctor --" })
                    .ToList();

                if (vm.WorkDate == default) vm.WorkDate = DateTime.UtcNow.Date;
                return View(vm);
            }

            var pid = await _userCtx.GetPatientIdAsync(_user.UserId, ct);
            if (pid is null) return Forbid();

            // Ensure UTC kind (input strings include 'Z', so they should already be UTC)
            var startUtc = vm.StartUtc.Kind == DateTimeKind.Utc
                ? vm.StartUtc
                : DateTime.SpecifyKind(vm.StartUtc, DateTimeKind.Utc);

            var endUtc = vm.EndUtc.Kind == DateTimeKind.Utc
                ? vm.EndUtc
                : DateTime.SpecifyKind(vm.EndUtc, DateTimeKind.Utc);

            var req = new CreateAppointmentRequest
            {
                DoctorId = vm.DoctorId,
                PatientId = pid.Value,
                StartUtc = startUtc,
                EndUtc = endUtc
            };

            await _appointments.BookAsync(req, ct);
            return RedirectToAction("Index", "Appointments", new { area = "Patient" });
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Slots([FromQuery] int doctorId, [FromQuery] string date, CancellationToken ct)
        {
            if (doctorId <= 0 || string.IsNullOrWhiteSpace(date))
                return BadRequest(new { message = "Invalid doctorId or date." });

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var parsed))
                return BadRequest(new { message = "Bad date format. Expect yyyy-MM-dd." });

            var workDate = parsed.Date;

            try
            {
                var slots = await _schedule.ListAvailableSlotsAsync(doctorId, workDate, ct)
                            ?? Array.Empty<ScheduleSlotDto>();

                var result = slots.Select(s => new
                {
                    startUtc = (s.StartUtc.Kind == DateTimeKind.Utc ? s.StartUtc : s.StartUtc.ToUniversalTime()).ToString("o"),
                    endUtc = (s.EndUtc.Kind == DateTimeKind.Utc ? s.EndUtc : s.EndUtc.ToUniversalTime()).ToString("o")
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Slots failed. doctorId={DoctorId}, date={Date}", doctorId, date);
                var msg = _env.IsDevelopment() ? ex.ToString() : "Server error while loading slots.";
                return StatusCode(500, new { message = msg });
            }
        }
    }
}
