using System;

namespace CST8002.Application.DTOs
{
    public sealed class DoctorAppointmentReportDto
    {
        public long ApptId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneMasked { get; set; }
    }
}