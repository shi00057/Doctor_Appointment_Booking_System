using System;
using System.Collections.Generic;

namespace CST8002.Web.Areas.Admin.ViewModels.Appointments
{
    public sealed class AdminAppointmentsIndexVm
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<object> Items { get; set; } = new();
    }
}
