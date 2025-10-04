using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CST8002.Application.DTOs;

namespace CST8002.Web.Models.Admin
{
    public class ManageAppointmentsVm
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? FromUtc { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ToUtc { get; set; }
        public IEnumerable<AppointmentDto> Items { get; set; }
    }
}
