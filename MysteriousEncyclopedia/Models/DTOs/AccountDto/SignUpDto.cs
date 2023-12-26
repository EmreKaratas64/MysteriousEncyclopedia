using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.AccountDto
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter a mail address")]
        [EmailAddress(ErrorMessage = "Please enter a vaild mail address")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("password", ErrorMessage = "Passwords does not match")]
        public string? passwordConfirm { get; set; }
    }
}
