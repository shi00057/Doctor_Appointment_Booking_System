using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CST8002.Web.Areas.Admin.ViewModels.Schedule
{
    public sealed class GenerateSlotsRangeVm
    {
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Range(0, 24)]
        public byte StartHour { get; set; }

        [Range(0, 24)]
        public byte EndHour { get; set; }

        public IEnumerable<SelectListItem> DoctorItems { get; set; } = Array.Empty<SelectListItem>();
    }
}
