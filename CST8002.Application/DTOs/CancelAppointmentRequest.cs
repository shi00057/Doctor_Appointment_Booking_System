namespace CST8002.Application.DTOs
{
    public sealed class CancelAppointmentRequest
    {
        public long ApptId { get; set; }
        public int PatientId { get; set; }
        public int? ByUserId { get; set; }
    }
}