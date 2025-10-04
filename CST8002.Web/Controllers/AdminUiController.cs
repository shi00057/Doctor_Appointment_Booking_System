using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Models.Admin;

namespace CST8002.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUiController : Controller
    {
        private readonly IScheduleService _schedule;
        private readonly IAppointmentService _appt;
        private readonly IReportService _report;

        public AdminUiController(IScheduleService schedule, IAppointmentService appt, IReportService report)
        {
            _schedule = schedule;
            _appt = appt;
            _report = report;
        }

        [HttpGet]
        public IActionResult ActivatePatients() => View();

        [HttpGet]
        public IActionResult RegisterDoctor() => View();

        [HttpGet]
        public IActionResult EditDoctors() => View();

        [HttpGet]
        public IActionResult EditPatients() => View();

        [HttpGet]
        public IActionResult GenerateSlots()
        {
            var vm = new GenerateSlotsVm
            {
                WorkDate = DateTime.Today,
                StartHour = 9,
                EndHour = 17
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateSlots(GenerateSlotsVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);
            var req = new GenerateSlotsRequest
            {
                DoctorId = vm.DoctorId,
                WorkDate = vm.WorkDate,
                StartHour = vm.StartHour,
                EndHour = vm.EndHour
            };
            await _schedule.AdminGenerateSlotsAsync(req, ct);
            TempData["ok"] = "Generated.";
            return RedirectToAction(nameof(GenerateSlots));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateSlotsRange(GenerateSlotsRangeVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(GenerateSlots));
            await _schedule.AdminGenerateSlotsRangeAsync(vm.DoctorId, vm.FromDate, vm.ToDate, vm.StartHour, vm.EndHour, ct);
            TempData["ok"] = "Generated for range.";
            return RedirectToAction(nameof(GenerateSlots));
        }

        [HttpGet]
        public IActionResult ManageAppointments() => View(new ManageAppointmentsVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAppointments(ManageAppointmentsVm vm, CancellationToken ct)
        {
            vm.Items = Enumerable.Empty<AppointmentDto>();
            if (vm.DoctorId.HasValue && vm.DoctorId.Value > 0)
                vm.Items = await _appt.ListByDoctorAsync(vm.DoctorId.Value, vm.FromUtc, vm.ToUtc, ct);
            else if (vm.PatientId.HasValue && vm.PatientId.Value > 0)
                vm.Items = await _appt.ListByPatientAsync(vm.PatientId.Value, vm.FromUtc, vm.ToUtc, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCancel(long apptId, string reason, CancellationToken ct)
        {
            await _appt.AdminCancelAsync(apptId, reason ?? string.Empty, ct);
            TempData["ok"] = "Appointment cancelled.";
            return RedirectToAction(nameof(ManageAppointments));
        }

        [HttpGet]
        public IActionResult Reports()
        {
            var vm = new ReportFilterVm { FromUtc = DateTime.UtcNow.Date.AddDays(-7), ToUtc = DateTime.UtcNow.Date };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reports(ReportFilterVm filter, CancellationToken ct)
        {
            var rows = await _report.ReportDoctorAppointmentsAsync(filter.DoctorId, filter.FromUtc, filter.ToUtc, ct);
            var totals = await _report.ReportDoctorTotalsAsync(filter.DoctorId, filter.FromUtc, filter.ToUtc, ct);
            var vm = new ReportResultVm { Filter = filter, Rows = rows, Totals = totals };
            return View(vm);
        }
    }
}
