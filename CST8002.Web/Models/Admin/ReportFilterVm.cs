using System;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Models.Admin
{
    public class ReportFilterVm
    {
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FromUtc { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ToUtc { get; set; }
    }
}
