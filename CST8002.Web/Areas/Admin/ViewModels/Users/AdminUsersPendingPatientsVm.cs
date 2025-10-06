using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Areas.Admin.ViewModels.Users
{
    public sealed class AdminUsersPendingPatientsVm
    {
        public List<PendingPatientItemVm> Pending { get; set; } = new();
    }

    public sealed class PendingPatientItemVm
    {
        [Required]
        public int UserId { get; set; }

        public int PatientId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool IsActive { get; set; }
    }
}
