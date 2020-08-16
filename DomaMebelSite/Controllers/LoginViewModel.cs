using System.ComponentModel.DataAnnotations;

namespace DomaMebelSite.Controllers
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ReturnUrl { get; set; }
    }
}