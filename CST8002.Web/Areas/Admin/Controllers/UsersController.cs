using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels.Users;
using AutoMapper;

namespace CST8002.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : AdminControllerBase
    {
        private readonly IUserService _users;
        private readonly IDoctorService _doctors;
        private readonly IPatientService _patients;
        private readonly IMapper _mapper;

        public UsersController(IUserService users, IDoctorService doctors, IPatientService patients, IMapper mapper)
        {
            _users = users;
            _doctors = doctors;
            _patients = patients;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PendingPatients(CancellationToken ct)
        {
            var list = await _users.ListPendingPatientsAsync(ct);
            var vm = new AdminUsersPendingPatientsVm
            {
                Pending = _mapper.Map<List<PendingPatientItemVm>>(list)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int userId, CancellationToken ct)
        {
            if (userId <= 0) return BadRequest();
            await _users.ActivateAsync(userId, ct);
            TempData["Success"] = "Patient activated.";
            return RedirectToAction(nameof(PendingPatients));
        }

        [HttpGet]
        public IActionResult Register() => View(new AdminUsersRegisterDoctorVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AdminUsersRegisterDoctorVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var salt = RandomNumberGenerator.GetBytes(32);
            var hash = Pbkdf2(vm.Password, salt, 100_000, 32);

            var (userId, doctorId) = await _doctors.CreateAsync(
                vm.Email.Trim(), hash, salt,
                vm.Name.Trim(), vm.Specialty?.Trim() ?? string.Empty,
                vm.Phone?.Trim() ?? string.Empty,
                vm.IsActive, ct);

            TempData["Success"] = $"Doctor created. UserId={userId}, DoctorId={doctorId}";
            return RedirectToAction(nameof(EditDoctor), new { id = doctorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditDoctor(int id, CancellationToken ct)
        {
            if (id <= 0) return NotFound();
            var dto = await _users.GetDoctorByIdAsync(id, ct);
            if (dto is null) return NotFound();

            var vm = new AdminUsersEditDoctorVm
            {
                DoctorId = dto.DoctorId,
                Name = dto.Name,
                Specialty = dto.Specialty,
                Email = dto.Email,
                Phone = dto.Phone,
                IsActive = dto.IsActive
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(AdminUsersEditDoctorVm vm, CancellationToken ct)
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
            var saved = await _doctors.UpdateAsync(dto, ct);
            TempData["Success"] = "Doctor saved.";
            return RedirectToAction(nameof(EditDoctor), new { id = saved.DoctorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditPatient(int id, CancellationToken ct)
        {
            if (id <= 0) return NotFound();
            var dto = await _users.GetPatientByIdAsync(id, ct);
            if (dto is null) return NotFound();

            var vm = new AdminUsersEditPatientVm
            {
                PatientId = dto.PatientId,
                FullName = dto.FullName,
                Phone = dto.Phone
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(AdminUsersEditPatientVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new PatientDto
            {
                PatientId = vm.PatientId,
                FullName = vm.FullName,
                Phone = vm.Phone
            };
            await _patients.UpdateAsync(dto, ct);

            TempData["Success"] = "Patient saved.";
            return RedirectToAction(nameof(EditPatient), new { id = vm.PatientId });
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int numBytes)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, System.Security.Cryptography.HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(numBytes);
        }
    }
}
