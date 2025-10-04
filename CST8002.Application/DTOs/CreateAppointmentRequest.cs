using System;

namespace CST8002.Application.DTOs
{
    public sealed class CreateAppointmentRequest
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int? ByUserId { get; set; }
    }
}