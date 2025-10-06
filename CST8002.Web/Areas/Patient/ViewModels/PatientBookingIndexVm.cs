using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Areas.Patient.ViewModels
{
    public sealed class PatientBookingIndexVm
    {
        public int DoctorId { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public List<SelectListItem> Doctors { get; set; } = new();
        public IEnumerable<SlotVm> Slots { get; set; } = Array.Empty<SlotVm>();
    }
}
