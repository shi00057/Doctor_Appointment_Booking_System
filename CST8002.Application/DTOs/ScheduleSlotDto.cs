
namespace CST8002.Application.DTOs
{
    public sealed class ScheduleSlotDto
    {
        public long SlotId { get; set; }
        public int DoctorId { get; set; }
        public System.DateTime StartUtc { get; set; }
        public System.DateTime EndUtc { get; set; }
    }
}
