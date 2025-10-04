using System;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Models.Admin
{
    public class GenerateSlotsVm
    {
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }
        [Range(0, 23)]
        public byte StartHour { get; set; }
        [Range(1, 24)]
        public byte EndHour { get; set; }
    }
}
