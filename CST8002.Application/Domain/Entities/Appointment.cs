using System;

namespace CST8002.Application.Domain.Entities
{
    public sealed class Appointment
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

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}