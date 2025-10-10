using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CST8002.Application.Abstractions;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Patient.ViewModels;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public sealed class ProfileController : PatientControllerBase
    {
        private readonly IPatientService _patients;
        private readonly IUserContextService _userCtx;
        private readonly ICurrentUser _user;

        public ProfileController(IPatientService patients, IUserContextService userCtx, ICurrentUser user)
        {
            _patients = patients;
            _userCtx = userCtx;
            _user = user;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(CancellationToken ct)
        {
            var pid = await _userCtx.GetPatientIdAsync(_user.UserId, ct);
            if (pid is null) return Forbid();

            var dto = await _patients.GetAsync(pid.Value, ct);
            if (dto is null) return NotFound();

            var vm = new PatientProfileEditVm
            {
                PatientId = dto.PatientId,
                FullName = dto.FullName,
                Phone = dto.Phone
            };
            return View(vm);
        }  
    }
}
