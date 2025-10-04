using System;

namespace CST8002.Application.Domain.Entities
{
    public sealed class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}