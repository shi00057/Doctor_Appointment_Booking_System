using System;
namespace CST8002.Web.Areas.Doctor.ViewModels
{
    public sealed class DoctorScheduleIndexVm
    {
        public DateTime WorkDate { get; set; } = DateTime.Today;
        public byte StartHour { get; set; } = 9;
        public byte EndHour { get; set; } = 17;
        public string? Message { get; set; }
    }
}
