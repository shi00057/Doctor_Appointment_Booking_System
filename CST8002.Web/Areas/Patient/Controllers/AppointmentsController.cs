using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Patient.ViewModels;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public sealed class AppointmentsController : PatientControllerBase
    {
        private readonly IAppointmentService _appointments;
        private readonly IUserContextService _userCtx;
        private readonly ICurrentUser _user;

        public AppointmentsController(
            IAppointmentService appointments,
            IUserContextService userCtx,
            ICurrentUser user)
        {
            _appointments = appointments;
            _userCtx = userCtx;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? from, DateTime? to, CancellationToken ct)
        {
            var pid = await _userCtx.GetPatientIdAsync(_user.UserId, ct);
            if (pid is null) return Forbid();

            var items = await _appointments.ListByPatientAsync(pid.Value, from, to, ct);
            var vm = new PatientAppointmentsIndexVm
            {
                From = from,
                To = to,
                Items = items.Select(x => new AppointmentVm
                {
                    ApptId = x.ApptId,
                    DoctorId = x.DoctorId.GetValueOrDefault(),
                    PatientId = x.PatientId.GetValueOrDefault(),
                    StartUtc = x.StartUtc,
                    EndUtc = x.EndUtc,
                    Status = x.Status
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(long apptId, CancellationToken ct)
        {
            var pid = await _userCtx.GetPatientIdAsync(_user.UserId, ct);
            if (pid is null) return Forbid();
            await _appointments.CancelByPatientAsync(apptId, pid.Value, ct);
            return RedirectToAction(nameof(Index));
        }
    }
}
