using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Doctor.ViewModels;
using CST8002.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public sealed class AppointmentsController : DoctorControllerBase
    {
        private readonly IAppointmentService _appointments;
        private readonly IDoctorService _doctors;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentService appointments, IDoctorService doctors, IMapper mapper)
        {
            _appointments = appointments;
            _doctors = doctors;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? from, DateTime? to, CancellationToken ct)
        {
            var doctorId = await ResolveDoctorIdAsync(ct);
            var rows = await _appointments.ListByDoctorAsync(doctorId, from, to, ct);
            var vm = new DoctorAppointmentsIndexVm
            {
                From = from,
                To = to,
                Items = rows.Select(_mapper.Map<AppointmentVm>).ToArray()
            };
            return View(vm);
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
