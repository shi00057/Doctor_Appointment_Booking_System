using System;
using System.Collections.Generic;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Areas.Doctor.ViewModels
{
    public sealed class DoctorAppointmentsIndexVm
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public IEnumerable<AppointmentVm> Items { get; set; } = Array.Empty<AppointmentVm>();
    }
}
