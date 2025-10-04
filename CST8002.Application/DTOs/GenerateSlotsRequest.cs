using System;

namespace CST8002.Application.DTOs
{
    public sealed class GenerateSlotsRequest
    {
        public int DoctorId { get; set; }
        public DateTime WorkDate { get; set; }
        public byte StartHour { get; set; }
        public byte EndHour { get; set; }
        public int? ByUserId { get; set; }
    }
}