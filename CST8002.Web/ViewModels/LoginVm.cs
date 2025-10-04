using System.ComponentModel.DataAnnotations;

namespace CST8002.Web.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
