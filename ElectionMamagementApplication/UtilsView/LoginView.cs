using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectionMamagementApplication.UtilsView
{
    public class LoginView
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string? Password { get; set; }

       
    }
}
