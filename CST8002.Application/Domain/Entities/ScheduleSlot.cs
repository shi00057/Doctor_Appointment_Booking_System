using System;

namespace CST8002.Application.Domain.Entities
{
    public sealed class ScheduleSlot
    {
        public long SlotId { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public bool IsAvailable { get; set; }
        public string Source { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}