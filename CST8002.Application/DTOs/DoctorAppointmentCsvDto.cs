using System;

namespace CST8002.Application.DTOs
{
    public sealed class DoctorAppointmentCsvDto
    {
        public long ApptId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}