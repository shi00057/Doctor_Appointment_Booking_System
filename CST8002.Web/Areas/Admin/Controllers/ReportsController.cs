using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CST8002.Application.Interfaces.Services;
using CST8002.Web.Areas.Admin.ViewModels.Reports;

namespace CST8002.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public sealed class ReportsController : AdminControllerBase 
    {
        private readonly IReportService _reports;

        public ReportsController(IReportService reports)
        {
            _reports = reports;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? doctorId, DateTime? from, DateTime? to, CancellationToken ct)
        {
            var vm = new AdminReportsIndexVm
            {
                DoctorId = doctorId ?? 0,
                From = from ?? DateTime.UtcNow.Date.AddDays(-7),
                To = to ?? DateTime.UtcNow.Date
            };

            if (vm.DoctorId > 0)
            {
                vm.Totals = await _reports.ReportDoctorTotalsAsync(vm.DoctorId, vm.From, vm.To, ct);
                var rows = await _reports.ReportDoctorAppointmentsAsync(vm.DoctorId, vm.From, vm.To, ct);
                vm.Rows = rows.Cast<object>().ToList();
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCsv(int doctorId, DateTime from, DateTime to, CancellationToken ct)
        {
            var rows = await _reports.ReportDoctorAppointmentsCsvAsync(doctorId, from, to, ct);
            var list = rows.Cast<object>().ToList();

            var sb = new StringBuilder();
            if (list.Count > 0)
            {
                var props = list[0].GetType().GetProperties();
                sb.AppendLine(string.Join(",", props.Select(p => p.Name)));
                foreach (var r in list)
                {
                    var vals = props.Select(p =>
                    {
                        var v = p.GetValue(r);
                        var s = v is DateTime dt ? dt.ToString("yyyy-MM-dd HH:mm") : v?.ToString() ?? "";
                        s = s.Replace("\"", "\"\"");
                        return $"\"{s}\"";
                    });
                    sb.AppendLine(string.Join(",", vals));
                }
            }

            var bytes = new UTF8Encoding(true).GetBytes(sb.ToString());
            var fileName = $"doctor_{doctorId}_appointments_{from:yyyyMMdd}_{to:yyyyMMdd}.csv";
            return File(bytes, "text/csv", fileName);
        }
    }
}
