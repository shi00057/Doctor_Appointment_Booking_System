using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CST8002.Application.DTOs;

namespace CST8002.Web.Models
{
    public class GenerateSlotsVm
    {
        [Required]
        public int DoctorId { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }
        [Range(0, 23)]
        public byte StartHour { get; set; }
        [Range(1, 24)]
        public byte EndHour { get; set; }
        public string Message { get; set; }
    }

    public class MyAppointmentsVm
    {
        public int DoctorId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IList<AppointmentDto> Items { get; set; }
    }

    public class DoctorProfileVm
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Specialty { get; set; }
        [EmailAddress]
        [MaxLength(320)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
    }
}
