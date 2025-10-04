
namespace CST8002.Application.DTOs
{
    public sealed class DoctorAppointmentCsvRowDto
    {
        public long ApptId { get; set; }
        public System.DateTime StartUtc { get; set; }
        public System.DateTime EndUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
