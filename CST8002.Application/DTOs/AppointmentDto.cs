using System;

namespace CST8002.Application.DTOs
{
    public sealed class AppointmentDto
    {
        public long ApptId { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
    }
}