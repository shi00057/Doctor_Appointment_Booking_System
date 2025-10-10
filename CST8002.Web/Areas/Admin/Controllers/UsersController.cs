using AutoMapper;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CST8002.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
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
        public IActionResult Register()
        {
            var vm = new AdminUsersRegisterDoctorVm { IsActive = true };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AdminUsersRegisterDoctorVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            const int saltSize = 16;
            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var password = vm.Password ?? string.Empty;

            using var sha256 = SHA256.Create();
            var hashInput = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
            var hash = sha256.ComputeHash(hashInput);

            try
            {
                var (userId, doctorId) = await _doctors.CreateAsync(
                    vm.Email.Trim(),
                    hash,
                    salt,
                    vm.Name.Trim(),
                    vm.Specialty?.Trim() ?? string.Empty,
                    vm.Phone?.Trim() ?? string.Empty,
                    vm.IsActive,
                    ct);

                TempData["Success"] = $"Doctor created. UserId={userId}, DoctorId={doctorId}";
                return RedirectToAction(nameof(Register));
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 50022)
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already exists.");
                return View(vm);
            }
        }
    }
}
