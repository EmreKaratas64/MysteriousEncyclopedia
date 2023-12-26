using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.AccountDto
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        public string password { get; set; }
    }
}
