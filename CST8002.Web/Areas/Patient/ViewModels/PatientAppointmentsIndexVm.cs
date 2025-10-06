using System;
using System.Collections.Generic;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Areas.Patient.ViewModels
{
    public sealed class PatientAppointmentsIndexVm
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<AppointmentVm> Items { get; set; } = new();
    }
}
