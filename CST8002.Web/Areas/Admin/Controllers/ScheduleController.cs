using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CST8002.Application.DTOs;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels.Schedule;

namespace CST8002.Web.Areas.Admin.Controllers
{

    public sealed class ScheduleController : AdminControllerBase
    {
        private readonly IScheduleService _schedule;
        private readonly IDoctorService _doctors;

        public ScheduleController(IScheduleService schedule, IDoctorService doctors)
        {
            _schedule = schedule;
            _doctors = doctors;
        }

        [HttpGet]
        public async Task<IActionResult> Generate(CancellationToken ct)
        {
            var doctorItems = (await _doctors.ListBasicsAsync(ct))
                .Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = string.IsNullOrWhiteSpace(d.Specialty) ? d.Name : $"{d.Name} ({d.Specialty})"
                })
                .ToList();

            var single = new GenerateSlotsVm
            {
                DoctorId = doctorItems.Any() ? int.Parse(doctorItems.First().Value) : 0,
                WorkDate = DateTime.Today,
                StartHour = 9,
                EndHour = 17,
                DoctorItems = doctorItems
            };

            var range = new GenerateSlotsRangeVm
            {
                DoctorId = single.DoctorId,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(7),
                StartHour = 9,
                EndHour = 17,
                DoctorItems = doctorItems
            };

            ViewBag.RangeVm = range;
            return View(single);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(GenerateSlotsVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                vm.DoctorItems = await BuildDoctorItems(ct);
                ViewBag.RangeVm = new GenerateSlotsRangeVm
                {
                    DoctorId = vm.DoctorId,
                    FromDate = vm.WorkDate,
                    ToDate = vm.WorkDate,
                    StartHour = vm.StartHour,
                    EndHour = vm.EndHour,
                    DoctorItems = vm.DoctorItems
                };
                return View(vm);
            }

            var req = new GenerateSlotsRequest
            {
                DoctorId = vm.DoctorId,
                WorkDate = vm.WorkDate.Date,
                StartHour = vm.StartHour,
                EndHour = vm.EndHour
            };

            await _schedule.AdminGenerateSlotsAsync(req, ct);
            TempData["ok"] = "Generated slots for selected day.";
            return RedirectToAction(nameof(Generate));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRange(GenerateSlotsRangeVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                vm.DoctorItems = await BuildDoctorItems(ct);
                var single = new GenerateSlotsVm
                {
                    DoctorId = vm.DoctorId,
                    WorkDate = vm.FromDate,
                    StartHour = vm.StartHour,
                    EndHour = vm.EndHour,
                    DoctorItems = vm.DoctorItems
                };
                ViewBag.RangeVm = vm;
                return View("Generate", single);
            }

            await _schedule.AdminGenerateSlotsRangeAsync(
                vm.DoctorId, vm.FromDate.Date, vm.ToDate.Date, vm.StartHour, vm.EndHour, ct);

            TempData["ok"] = "Generated slots for date range.";
            return RedirectToAction(nameof(Generate));
        }

        private async Task<System.Collections.Generic.List<SelectListItem>> BuildDoctorItems(CancellationToken ct)
        {
            var doctors = await _doctors.ListBasicsAsync(ct);
            return doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorId.ToString(),
                Text = string.IsNullOrWhiteSpace(d.Specialty) ? d.Name : $"{d.Name} ({d.Specialty})"
            }).ToList();
        }
    }
}
