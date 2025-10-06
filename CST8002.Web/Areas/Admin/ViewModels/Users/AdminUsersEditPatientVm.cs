using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Areas.Admin.ViewModels.Users
{
    public sealed class AdminUsersEditPatientVm
    {
        [Required]
        public int PatientId { get; set; }

        [Required, Display(Name = "Full Name")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        [StringLength(30)]
        public string? Phone { get; set; }
    }
}
