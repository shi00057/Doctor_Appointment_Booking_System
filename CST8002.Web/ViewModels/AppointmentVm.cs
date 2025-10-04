using System;

namespace CST8002.Web.ViewModels
{
    public class AppointmentVm
    {
        public long ApptId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
    }
}
