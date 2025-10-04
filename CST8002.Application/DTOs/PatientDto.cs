using System;

namespace CST8002.Application.DTOs
{
    public sealed class PatientDto
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
