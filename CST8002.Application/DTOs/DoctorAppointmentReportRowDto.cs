
namespace CST8002.Application.DTOs
{
    public sealed class DoctorAppointmentReportRowDto
    {
        public long ApptId { get; set; }
        public System.DateTime StartUtc { get; set; }
        public System.DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
    }
}
