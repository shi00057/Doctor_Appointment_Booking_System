// Areas/Doctor/ViewModels/DoctorScheduleIndexVm.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Areas.Doctor.ViewModels
{
    public sealed class DoctorScheduleIndexVm
    {
        [Required, DataType(DataType.Date)]
        public DateTime? WorkDate { get; set; }

        [Required, Range(0, 23, ErrorMessage = "StartHour must be 0–23")]
        public byte? StartHour { get; set; }

        [Required, Range(1, 24, ErrorMessage = "EndHour must be 1–24")]
        public byte? EndHour { get; set; }

        public string? Message { get; set; }
    }
}
