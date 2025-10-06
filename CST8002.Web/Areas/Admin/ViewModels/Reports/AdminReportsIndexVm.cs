using System;
using System.Collections.Generic;
using CST8002.Application.DTOs;

namespace CST8002.Web.Areas.Admin.ViewModels.Reports
{
    public sealed class AdminReportsIndexVm
    {
        public int DoctorId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DoctorTotalsDto? Totals { get; set; }
        public List<object> Rows { get; set; } = new();
    }
}
