using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CST8002.Web.ViewModels;
using CST8002.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _users;

        public AccountController(IUserRepository users)
        {
            _users = users;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var email = vm.UserName?.Trim() ?? string.Empty;
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
            if (result.UserId <= 0 || result.IsActive == false)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials or inactive user");
                return View(vm);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, result.Role ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (string.Equals(result.Role, "Admin", StringComparison.OrdinalIgnoreCase)) return RedirectToAction("Admin", "Dashboard");
            if (string.Equals(result.Role, "Doctor", StringComparison.OrdinalIgnoreCase)) return RedirectToAction("Doctor", "Dashboard");
            return RedirectToAction("Patient", "Dashboard");
        }

        [HttpGet]
        public IActionResult RegisterPatient()
        {
            return View(new RegisterPatientVm());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient(RegisterPatientVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var salt = RandomNumberGenerator.GetBytes(16);
            var pwd = vm.Password ?? string.Empty;
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(salt.Concat(Encoding.UTF8.GetBytes(pwd)).ToArray());

            var userId = await _users.RegisterPatientAsync(
                vm.Email?.Trim() ?? string.Empty,
                hash,
                salt,
                vm.FullName?.Trim() ?? string.Empty,
                vm.Phone?.Trim() ?? string.Empty,
                ct);

            if (userId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Registration failed");
                return View(vm);
            }

            return RedirectToAction("RegisterSuccess");
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("LoggedOut");
        }
    }
}
