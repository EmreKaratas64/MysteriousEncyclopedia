using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.AccountDto
{
    public class PasswordReset
    {
        [Required(ErrorMessage = "Please enter your mail!")]
        public string email { get; set; }
    }
}
