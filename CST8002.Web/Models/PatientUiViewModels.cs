using CST8002.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Models
{
    public class CreateAppointmentVm
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }
        public List<SelectListItem> Doctors { get; set; } = new List<SelectListItem>();
        public List<ScheduleSlotDto> Slots { get; set; } = new List<ScheduleSlotDto>();
    }

    public class PatientAppointmentsVm
    {
        public int PatientId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IList<AppointmentDto> Items { get; set; }
    }

    public class PatientProfileVm
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }
        
        public string Message { get; set; }
    }
}
