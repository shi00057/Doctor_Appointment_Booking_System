using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ui/directory")]
    public class DirectoryController : ControllerBase
    {
        private readonly IDoctorRepository _doctors;
        private readonly IScheduleRepository _schedules;

        public DirectoryController(IDoctorRepository doctors, IScheduleRepository schedules)
        {
            _doctors = doctors;
            _schedules = schedules;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> Doctors(CancellationToken ct)
        {
            var list = await _doctors.ListBasicsAsync(ct);
            return Ok(list);
        }

        [HttpGet("slots")]
        public async Task<IActionResult> Slots(int doctorId, DateTime? date, CancellationToken ct)
        {
            var day = (date ?? DateTime.UtcNow).Date;
            var slots = await _schedules.ListAvailableSlotsAsync(doctorId, day, ct);
            var data = slots.Select(s => new { s.DoctorId, s.StartUtc, s.EndUtc });
            return Ok(data);
        }
    }
}
