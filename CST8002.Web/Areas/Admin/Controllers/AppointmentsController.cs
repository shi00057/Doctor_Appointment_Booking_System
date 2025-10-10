using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels.Appointments;

namespace CST8002.Web.Areas.Admin.Controllers
{

    public sealed class AppointmentsController : AdminControllerBase
    {
        private readonly IAppointmentService _appointments;

        public AppointmentsController(IAppointmentService appointments)
        {
            _appointments = appointments;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? doctorId, int? patientId, DateTime? from, DateTime? to, CancellationToken ct)
        {
            var vm = new AdminAppointmentsIndexVm
            {
                DoctorId = doctorId,
                PatientId = patientId,
                From = from,
                To = to
            };

            if (doctorId.HasValue && doctorId.Value > 0)
            {
                var list = await _appointments.ListByDoctorAsync(doctorId.Value, from, to, ct);
                vm.Items = list.Cast<object>().ToList();
            }
            else if (patientId.HasValue && patientId.Value > 0)
            {
                var list = await _appointments.ListByPatientAsync(patientId.Value, from, to, ct);
                vm.Items = list.Cast<object>().ToList();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(long apptId, string? reason, CancellationToken ct)
        {
            await _appointments.AdminCancelAsync(apptId, reason ?? string.Empty, ct);
            return RedirectToAction(nameof(Index));
        }
    }
}
