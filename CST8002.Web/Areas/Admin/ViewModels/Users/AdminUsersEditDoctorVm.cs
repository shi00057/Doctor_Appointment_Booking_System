using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.Areas.Admin.ViewModels.Users
{
    public sealed class AdminUsersEditDoctorVm
    {
        [Required]
        public int DoctorId { get; set; }

        [Required, Display(Name = "Doctor Name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Specialty")]
        [StringLength(100)]
        public string? Specialty { get; set; }

        [EmailAddress, Display(Name = "Email")]
        [StringLength(320)]
        public string? Email { get; set; }

        [Display(Name = "Phone")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
