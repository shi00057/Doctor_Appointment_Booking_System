using System;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Models.Admin
{
    public class GenerateSlotsRangeVm
    {
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        [Range(0, 23)]
        public byte StartHour { get; set; }
        [Range(1, 24)]
        public byte EndHour { get; set; }
    }
}
