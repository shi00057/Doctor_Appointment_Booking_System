using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.ViewModels
{
    public class RegisterPatientVm
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
