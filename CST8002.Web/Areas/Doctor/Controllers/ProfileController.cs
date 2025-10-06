using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public sealed class ProfileController : DoctorControllerBase
    {
        private readonly IDoctorService _doctors;
        private readonly IMapper _mapper;

        public ProfileController(IDoctorService doctors, IMapper mapper)
        {
            _doctors = doctors;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(CancellationToken ct)
        {
            var doctor = await LoadCurrentDoctorAsync(ct);
            var vm = new DoctorProfileEditVm
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                Specialty = doctor.Specialty,
                Email = doctor.Email,
                Phone = doctor.Phone,
                IsActive = doctor.IsActive
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorProfileEditVm vm, CancellationToken ct)
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
            await _doctors.UpdateAsync(dto, ct);
            TempData["Ok"] = "Saved";
            return RedirectToAction(nameof(Edit));
        }

        private async Task<DoctorDto> LoadCurrentDoctorAsync(CancellationToken ct)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? string.Empty;
            var list = await _doctors.ListBasicsAsync(ct);
            return list.First(x => string.Equals(x.Email ?? string.Empty, email, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
