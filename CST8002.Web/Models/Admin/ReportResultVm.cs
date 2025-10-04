using System.Collections.Generic;
using CST8002.Application.DTOs;

namespace CST8002.Web.Models.Admin
{
    public class ReportResultVm
    {
        public ReportFilterVm Filter { get; set; }
        public IEnumerable<DoctorAppointmentReportRowDto> Rows { get; set; }
        public DoctorTotalsDto Totals { get; set; }
    }
}
