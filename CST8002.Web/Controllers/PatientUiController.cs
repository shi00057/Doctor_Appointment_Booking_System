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

namespace CST8002.Web.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientUiController : Controller
    {
        private readonly IScheduleService _schedule;
        private readonly IAppointmentService _appointments;
        private readonly IPatientService _patients;
        private readonly IDoctorService _doctors;
        private readonly IDateTimeProvider _clock;

        public PatientUiController(
            IScheduleService schedule,
            IAppointmentService appointments,
            IPatientService patients,
            IDoctorService doctors,
            IDateTimeProvider clock)
        {
            _schedule = schedule;
            _appointments = appointments;
            _patients = patients;
            _doctors = doctors;
            _clock = clock;
        }

        [HttpGet]
        public IActionResult Index(int patientId)
        {
            ViewBag.PatientId = patientId;
            ViewBag.Today = _clock.UtcNow.ToLocalTime().Date;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAppointment(int patientId, CancellationToken ct)
        {
            var doctors = await _doctors.ListBasicsAsync(ct);
            var vm = new Models.CreateAppointmentVm
            {
                PatientId = patientId,
                WorkDate = _clock.UtcNow.ToLocalTime().Date,
                DoctorId = doctors.FirstOrDefault()?.DoctorId ?? 0,
                Doctors = doctors.Select(d => new SelectListItem($"{d.Name} ({d.Specialty})", d.DoctorId.ToString())).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(Models.CreateAppointmentVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                var doctors = await _doctors.ListBasicsAsync(ct);
                vm.Doctors = doctors.Select(d => new SelectListItem($"{d.Name} ({d.Specialty})", d.DoctorId.ToString(), d.DoctorId == vm.DoctorId)).ToList();
                return View(vm);
            }

            var slots = await _schedule.ListAvailableSlotsAsync(vm.DoctorId, vm.WorkDate, ct);
            vm.Slots = slots.OrderBy(s => s.StartUtc).ToList();

            var doctors2 = await _doctors.ListBasicsAsync(ct);
            vm.Doctors = doctors2.Select(d => new SelectListItem($"{d.Name} ({d.Specialty})", d.DoctorId.ToString(), d.DoctorId == vm.DoctorId)).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(int patientId, int doctorId, string startUtc, string endUtc, CancellationToken ct)
        {
            var start = DateTime.Parse(startUtc, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var end = DateTime.Parse(endUtc, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var req = new CreateAppointmentRequest
            {
                PatientId = patientId,
                DoctorId = doctorId,
                StartUtc = start,
                EndUtc = end
            };

            await _appointments.BookAsync(req, ct);
            return RedirectToAction(nameof(MyAppointments), new { patientId });
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointments(int patientId, DateTime? from, DateTime? to, CancellationToken ct)
        {
            var now = _clock.UtcNow;
            var fromUtc = from?.ToUniversalTime() ?? now.AddDays(-30);
            var toUtc = to?.ToUniversalTime() ?? now.AddDays(30);
            var list = await _appointments.ListByPatientAsync(patientId, fromUtc, toUtc, ct);
            var vm = new Models.PatientAppointmentsVm
            {
                PatientId = patientId,
                From = fromUtc.ToLocalTime(),
                To = toUtc.ToLocalTime(),
                Items = list.OrderByDescending(x => x.StartUtc).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelMyAppointment(long apptId, int patientId, CancellationToken ct)
        {
            await _appointments.CancelByPatientAsync(apptId, patientId, ct);
            return RedirectToAction(nameof(MyAppointments), new { patientId });
        }

        [HttpGet]
        public IActionResult Profile(int patientId)
        {
            var vm = new Models.PatientProfileVm
            {
                PatientId = patientId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Models.PatientProfileVm vm, CancellationToken ct)
        {
            ModelState.Remove("Message");
            if (!ModelState.IsValid) return View(vm);

            var dto = new PatientDto
            {
                PatientId = vm.PatientId,
                FullName = vm.FullName,
                Phone = vm.Phone
            };
            var saved = await _patients.UpdateAsync(dto, ct);
            vm.FullName = saved.FullName;
            vm.Phone = saved.Phone;
            vm.Message = "Profile saved.";
            return View(vm);
        }
    }
}
