using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Web.ViewModels;
using System.Collections.Generic;
using CST8002.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _users;

        private const string RoleAdmin = "Admin";
        private const string RoleDoctor = "Doctor";
        private const string RolePatient = "Patient";

        public AccountController(IUserRepository users)
        {
            _users = users;
        }

        // ---------- Login ----------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            
            var email = (vm.UserName ?? string.Empty).Trim();
            var pwd = vm.Password ?? string.Empty;

            
            var salt = await _users.GetSaltByEmailAsync(email, ct);
            if (salt is null || salt.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials or inactive user");
                return View(vm);
            }

            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(salt.Concat(Encoding.UTF8.GetBytes(pwd)).ToArray());

            
            var result = await _users.LoginAsync(email, hash, ct);
            if (result.UserId <= 0 || result.IsActive == false || string.IsNullOrWhiteSpace(result.Role))
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials or inactive user");
                return View(vm);
            }

            // NameIdentifier/Name/Role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, result.Role!)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // Areas Dashboard/Index
            return RedirectByRole(result.Role!);
        }

        private IActionResult RedirectByRole(string role)
        {
            if (string.Equals(role, RoleAdmin, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            if (string.Equals(role, RoleDoctor, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard", new { area = "Doctor" });

            return RedirectToAction("Index", "Dashboard", new { area = "Patient" });
        }

        // ---------- Register Patient ----------
        private const int SaltSize = 16;
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterPatient()
        {
            return View(new RegisterPatientVm());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPatient(RegisterPatientVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            var pwd = vm.Password ?? string.Empty;
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(salt.Concat(Encoding.UTF8.GetBytes(pwd)).ToArray());

            var userId = await _users.RegisterPatientAsync(
                (vm.Email ?? string.Empty).Trim(),
                hash,
                salt,
                (vm.FullName ?? string.Empty).Trim(),
                (vm.Phone ?? string.Empty).Trim(),
                ct);

            if (userId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Registration failed");
                return View(vm);
            }

            return RedirectToAction(nameof(RegisterSuccess));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        // ---------- Logout ----------
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("LoggedOut");
        }
    }
}
