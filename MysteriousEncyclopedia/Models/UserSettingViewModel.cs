using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models
{
    public class UserSettingViewModel
    {
        [Required(ErrorMessage = "Please enter your password")]
        public string? currentpassword { get; set; }

        [Required(ErrorMessage = "Please enter a new password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("password", ErrorMessage = "Passwords does not match")]
        public string? passwordConfirm { get; set; }
    }
}
