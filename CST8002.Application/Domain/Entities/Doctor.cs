using System;

namespace CST8002.Application.Domain.Entities
{
    public sealed class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}