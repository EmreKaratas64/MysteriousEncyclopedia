using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.AccountDto
{
    public class PasswordRecover
    {
        [Required(ErrorMessage = "Please enter a password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("password", ErrorMessage = "Passwords does not match")]
        public string passwordConfirm { get; set; }
    }
}
